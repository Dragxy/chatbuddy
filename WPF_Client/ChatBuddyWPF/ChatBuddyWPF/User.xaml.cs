using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ChatBuddyWPF
{
    public partial class User : Page
    {

        public User()
        {
            InitializeComponent();
            LoadChatsAsync();
        }

        private async void LoadChatsAsync()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , MainWindow.JwtToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await httpClient.GetAsync($"http://localhost:8080/api/info/user/{MainWindow.Username}");

                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }

                UserInfo userinfo = await response.Content.ReadFromJsonAsync<UserInfo>();
                List<Chatroom> chatrooms = userinfo.Chatrooms;

                GenerateButtons(chatrooms);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private async void CreateChatAsync(string chatName)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , MainWindow.JwtToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestBody = new { chatName = chatName , username = MainWindow.Username };
                var jsonRequestBody = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(jsonRequestBody , Encoding.UTF8 , "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("http://localhost:8080/api/info/chats/add" , httpContent);

                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                LoadChatsAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void GenerateButtons(List<Chatroom> chatrooms)
        {
            ButtonContainer.Children.Clear();
            foreach(var chatroom in chatrooms)
            {
                CreateChatButton(chatroom);
            }
        }

        private void CreateChatButton(Chatroom chatroom)
        {
            Button button = new Button
            {
                Content = chatroom.Name ,
                Margin = new Thickness(0 , 25 , 0 , 0) ,
                Height = 35 ,
                Width = 250,
                Style = (Style)Application.Current.Resources["CustomButtonStyle"]
            };

            button.Click += (sender , e) => Button_Click(sender , e , chatroom);
            ButtonContainer.Children.Add(button);
        }

        private void Button_Click(object sender , RoutedEventArgs e , Chatroom chatroom)
        {
            MainWindow.ChatroomId = chatroom.Id;
            this.NavigationService.Navigate(new ChatPage(chatroom.Name));
        }

        private void CreateChatButton_Click(object sender , RoutedEventArgs e)
        {
            CreateChat dialog = new CreateChat();
            bool? result = dialog.ShowDialog();
            if(result == true)
            {
                string chatName = dialog.ChatName;
                CreateChatAsync(chatName);
            }
        }
    }

    public class UserInfo
    {
        [JsonPropertyName("chatrooms")]
        public List<Chatroom> Chatrooms { get; set; }
    }

}