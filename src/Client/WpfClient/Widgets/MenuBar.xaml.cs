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
    /// Завершает работу текущего экземпляра приложения в ответ на событие пользовательского интерфейса.
    /// </summary>
    private void ApplicationQuit(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();

    /// <summary>
    /// Перезапускает приложение, запуская новый экземпляр и завершая текущий.
    /// </summary>
    private void Restart(object sensder, RoutedEventArgs e)
    {
        Process.Start(Application.ResourceAssembly.Location);
        Application.Current.Shutdown();
    }

    /// <summary>
    /// Отображает информационное окно сообщения с деталями о приложении.
    /// </summary>
    private void About(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Здрасте...", "О нас", MessageBoxButton.OK, MessageBoxImage.Information);
}
