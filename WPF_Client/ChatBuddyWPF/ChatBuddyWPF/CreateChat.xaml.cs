using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ChatBuddyWPF
{
    public partial class CreateChat : Window
    {
        public string ChatName { get; private set; }

        public CreateChat()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void OKButton_Click(object sender , RoutedEventArgs e)
        {
            ChatName = ChatNameTextBox.Text;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender , RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
