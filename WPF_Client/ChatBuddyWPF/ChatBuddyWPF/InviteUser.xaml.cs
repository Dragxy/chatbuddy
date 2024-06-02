using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.Json.Serialization;

namespace ChatBuddyWPF
{
    public partial class InviteUser : Window
    {
        public string UserToInvite { get; private set; }
        public InviteUser()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void OKButton_Click(object sender , RoutedEventArgs e)
        {
            UserToInvite = UserNameTextBox.Text;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender , RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void UserNameTextBox_TextChanged(object sender , TextChangedEventArgs e)
        {
            string searchText = UserNameTextBox.Text;
            List<string> suggestions = await FetchSuggestionsFromApi(searchText);

            if(suggestions.Count > 0)
            {
                SuggestionsListBox.ItemsSource = suggestions;
                SuggestionsListBox.Visibility = Visibility.Visible;
            }
            else
            {
                SuggestionsListBox.Visibility = Visibility.Collapsed;
            }
        }

        private async Task<List<string>> FetchSuggestionsFromApi(string searchText)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"http://{ MainWindow.BaseUrl }");
                string endpoint = "/suggestion";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , MainWindow.JwtToken);
                HttpResponseMessage response = await client.GetAsync($"{endpoint}?searchstr={searchText}&chattarget={MainWindow.ChatroomId}");

                if(response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    SuggestionWrapper suggestionWrapper = JsonSerializer.Deserialize<SuggestionWrapper>(responseBody);
                    if(suggestionWrapper.Suggestions.Count == 0) {
                        suggestionWrapper.Suggestions.Add("No users found.");
                    }
                    return suggestionWrapper.Suggestions;
                }
                else
                    MessageBox.Show(($"Could not retrieve suggestions from Api."));
            }
            catch(HttpRequestException e)
            {
                MessageBox.Show(($"An error occurred: {e.Message}"));
            }
            return new List<string>();
        }
        private void SuggestionsListBox_SelectionChanged(object sender , RoutedEventArgs e)
        {
            if(SuggestionsListBox.SelectedItem != null)
            {
                UserNameTextBox.Text = SuggestionsListBox.SelectedItem.ToString();
            }
        }
    }
    public class SuggestionWrapper
    {
        [JsonPropertyName("suggestions")]
        public List<string> Suggestions { get; set; }
        public SuggestionWrapper()
        {
            Suggestions = new List<string>();
        }
    }
}
