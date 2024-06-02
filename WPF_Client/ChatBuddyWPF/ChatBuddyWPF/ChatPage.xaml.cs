using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http.Json;
using System.Text.Json;
using Netina.Stomp.Client.Interfaces;
using Netina.Stomp.Client;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Text.Json.Serialization;

namespace ChatBuddyWPF
{
    public partial class ChatPage : Page
    {
        private ObservableCollection<ChatMessageDisplay> chatMessages = new ObservableCollection<ChatMessageDisplay>();
        IStompClient _client;

        public ChatPage(string chatname)
        {
            InitializeComponent();
            MessageListBox.ItemsSource = chatMessages;
            textblock_chatname.Text = chatname;
            FetchChatHistoryFromApi();
            ConnectToWebsocket();
            chatMessages.CollectionChanged += ChatMessages_CollectionChanged;
        }

        public async void ConnectToWebsocket()
        {
                var webSocketUrl = $"ws://{MainWindow.BaseUrl}/ws/websocket";
                _client = new StompClient(webSocketUrl);
                var headers = new Dictionary<string , string>();
                headers.Add("Authorization" , "Bearer " + MainWindow.JwtToken);
            await _client.ConnectAsync(headers);

                await _client.SubscribeAsync<object>($"/topic/{MainWindow.ChatroomId}" , headers , async (sender , dto) =>
                {

                    AddMessageToDisplay(JsonSerializer.Deserialize<ChatMessage>(dto.ToString()));
                });
        }

        public async void FetchChatHistoryFromApi()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , MainWindow.JwtToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await httpClient.GetAsync($"http://{MainWindow.BaseUrl}/api/info/chat/{MainWindow.ChatroomId}");

                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }

                Chatroom chatroom = await response.Content.ReadFromJsonAsync<Chatroom>();
                foreach(ChatMessage chatMessage in chatroom.Messages)
                {
                   AddMessageToDisplay(chatMessage);
                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void AddMessageToDisplay(ChatMessage chatMessage)
        {
            ChatMessageDisplay chatMessageDisplay = new ChatMessageDisplay()
            {
                SenderName = chatMessage.Username ,
                Message = chatMessage.Content ,
                IsOwnMessage = chatMessage.Username.ToLower()==MainWindow.Username.ToLower(),
                IsEventMessage = chatMessage.Type == MessageType.JOIN || chatMessage.Type == MessageType.LEAVE
            };
            if(chatMessageDisplay.IsEventMessage)
            {
                if(chatMessage.Type == MessageType.JOIN)
                    chatMessageDisplay.Message = $"{chatMessage.Username} joined.";
                else if(chatMessage.Type == MessageType.LEAVE)
                    chatMessageDisplay.Message = $"{chatMessage.Username} left.";

            }

            Dispatcher.Invoke(() =>
            {
                chatMessages.Add(chatMessageDisplay);
            });
        }

        private async void InviteUserButton_Click(object sender , RoutedEventArgs e)
        {
            InviteUser dialog = new InviteUser();
            bool? result = dialog.ShowDialog();
            if(result == true)
            {
                string UserToInvite = dialog.UserToInvite;
                InviteUserToChatOverApi(UserToInvite);
            }
        }

        private async void InviteUserToChatOverApi(string username)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , MainWindow.JwtToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestBody = new {username = username};
                var jsonRequestBody = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(jsonRequestBody , Encoding.UTF8 , "application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"http://{MainWindow.BaseUrl}/api/info/inviteUser/"+MainWindow.ChatroomId , httpContent);

                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }

                string responseBody = await response.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ReturnUserPageButton_Click(object sender , RoutedEventArgs e)
        {

              this.NavigationService.Navigate(new User());
              MainWindow.ChatroomId = null;
              _client.DisconnectAsync();
        }

        private async void LeaveChatButton_Click(object sender , RoutedEventArgs e)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , MainWindow.JwtToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestBody = new { username = MainWindow.Username };
                var jsonRequestBody = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(jsonRequestBody , Encoding.UTF8 , "application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"http://{MainWindow.BaseUrl}/api/info/leaveChat/"+MainWindow.ChatroomId , httpContent);

                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                ReturnUserPageButton_Click(this, null);
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void SendButton_Click(object sender , RoutedEventArgs e)
        {
            ChatMessage chatMessage = new ChatMessage()
            {
                Content = textbox_send.Text ,
                Username = MainWindow.Username ,
                Type = MessageType.CHAT ,
            };
            var headers = new Dictionary<string , string>();
            headers.Add("Authorization" , "Bearer " + MainWindow.JwtToken);
            _client.SendAsync(JsonSerializer.Serialize(chatMessage) , $"/app/chat.send/{MainWindow.ChatroomId}" , headers);
            textbox_send.Text = "";
        }
        public class ChatMessageDisplay
        {
            public string SenderName { get; set; }
            public string Message { get; set; }
            public bool IsOwnMessage { get; set; }
            public bool IsEventMessage { get; set; }
            public string SenderInitial => string.IsNullOrEmpty(SenderName) ? "" : SenderName[0].ToString();
        }

        private void ChatMessages_CollectionChanged(object sender , NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                MessageListBox.ScrollIntoView(e.NewItems[0]);
            }
        }
    }
}
