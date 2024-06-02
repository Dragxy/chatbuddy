using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatBuddyWPF
{
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void textbox_signup_Pressed(object sender , RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Signup());
        }

        private void checkbox_showpassword_Checked(object sender , RoutedEventArgs e)
        {
            if(checkbox_showpassword.IsChecked == true)
            {
                textbox_password.Text = passwordbox_password.Password;
                passwordbox_password.Visibility = Visibility.Collapsed;
                textbox_password.Visibility = Visibility.Visible;
                textbox_password.Focus();
            }
            else
            {
                passwordbox_password.Password = textbox_password.Text;
                passwordbox_password.Visibility = Visibility.Visible;
                textbox_password.Visibility = Visibility.Collapsed;
                passwordbox_password.Focus();
            }
        }

        private async void LoginBtn_Click(object sender , RoutedEventArgs e)
        {
            string username = textbox_username.Text.Trim();
            string password;
            if(checkbox_showpassword.IsChecked == true)
                password = textbox_password.Text.Trim();
            else  
                password = passwordbox_password.Password.Trim();

            try
            {
                using(HttpClient client = new HttpClient())
                {
                    var requestData = new
                    {
                        username = username ,
                        password = password
                    };

                    string json = JsonSerializer.Serialize(requestData);
                    HttpContent content = new StringContent(json , Encoding.UTF8 , "application/json");

                    HttpResponseMessage response = await client.PostAsync($"http://{MainWindow.BaseUrl}/api/auth/signin" , content);

                    if(response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var data = JsonSerializer.Deserialize<AuthResponse>(responseBody);

                        // Authentication successful
                        string jwtToken = data.Token;
                        MainWindow.JwtToken = jwtToken;
                        MainWindow.Username = username;

                        // Hide the login form and show the user page
                        this.NavigationService.Navigate(new User());
                    }
                    else
                    {
                        throw new Exception("Authentication failed");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message , "Error" , MessageBoxButton.OK , MessageBoxImage.Error);
            }
        }
    }

    public class AuthResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
