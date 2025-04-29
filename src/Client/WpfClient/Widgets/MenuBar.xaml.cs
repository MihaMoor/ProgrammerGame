using System.Windows;
using System.Windows.Controls;

namespace WpfClient.Widgets.MenuBar;

/// <summary>
/// Interaction logic for MenuBar.xaml
/// </summary>
public partial class MenuBar : Page
{
    public MenuBar()
    {
        InitializeComponent();
    }

    private void ApplicationQuit(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();

    private void About(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Здрасте...", "О нас", MessageBoxButton.OK, MessageBoxImage.Information);
}
