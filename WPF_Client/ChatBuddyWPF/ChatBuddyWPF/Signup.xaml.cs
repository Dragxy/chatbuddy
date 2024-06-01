using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    public partial class Signup : Page
    {
        public Signup()
        {
            InitializeComponent();
        }

        private void textbox_login_Pressed(object sender, RoutedEventArgs e)
        {
            if(this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
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

        private async void SignUpBtn_Click(object sender , RoutedEventArgs e)
        {
            string username = textbox_username.Text.Trim();
            string email = textbox_username.Text.Trim();
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
                        email = email ,
                        password = password
                    };

                    string json = JsonSerializer.Serialize(requestData);
                    HttpContent content = new StringContent(json , Encoding.UTF8 , "application/json");

                    HttpResponseMessage response = await client.PostAsync("http://localhost:8080/api/auth/signup" , content);

                    if(response.IsSuccessStatusCode)
                    {
                        if(this.NavigationService.CanGoBack)
                        {
                            this.NavigationService.GoBack();
                        }
                    }
                    else
                    {
                        throw new Exception("Sign-up failed");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message , "Error" , MessageBoxButton.OK , MessageBoxImage.Error);
            }
        }
    }
}
