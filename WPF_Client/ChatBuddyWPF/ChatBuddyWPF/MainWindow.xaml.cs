using System.Text;
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
    public partial class MainWindow : Window
    {
        public static string JwtToken {  get; set; }
        public static string Username {  get; set; }
        public static string ChatroomId { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Login());
        }
    }
}