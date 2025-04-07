using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += AppStartup;
        }

        private void AppStartup(object sender, StartupEventArgs e)
        {
            // Настройка зависимостей
            AppServices.Configure();

            // Получение основного окна из DI-контейнера и запуск
            var mainWindow = AppServices.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
