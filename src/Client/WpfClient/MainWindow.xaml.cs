using System.Windows;
using WpfClient.Pages;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Game game)
        {
            InitializeComponent();
            MainFrame.Navigate(game);
        }
    }
}