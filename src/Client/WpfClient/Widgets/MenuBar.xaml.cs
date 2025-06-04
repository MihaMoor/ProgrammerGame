using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WpfClient.Widgets;

/// <summary>
/// Interaction logic for MenuBar.xaml
/// </summary>
public partial class MenuBar : Page
{
    public MenuBar()
    {
        InitializeComponent();
    }

    /// <summary>
        /// Shuts down the current application instance in response to a UI event.
        /// </summary>
        private void ApplicationQuit(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();

    /// <summary>
    /// Restarts the application by launching a new instance and shutting down the current one.
    /// </summary>
    private void Restart(object sensder, RoutedEventArgs e)
    {
        Process.Start(Application.ResourceAssembly.Location);
        Application.Current.Shutdown();
    }

    /// <summary>
        /// Displays an informational message box with details about the application.
        /// </summary>
        private void About(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Здрасте...", "О нас", MessageBoxButton.OK, MessageBoxImage.Information);
}
