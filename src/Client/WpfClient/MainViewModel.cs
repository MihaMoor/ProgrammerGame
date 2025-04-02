using Client.Domain;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfClient;

public class MainViewModel
{
    public ObservableCollection<MenuItem> ViewMainMenuItems { get; set; } = [];

    public MainViewModel()
    {
        ViewMainMenuItems = InitializeMenu();
    }

    private ObservableCollection<MenuItem> InitializeMenu()
        =>
        [
            new MenuItem()
                {
                    Header = "Файл",
                    SubItems =
                    [
                        new MenuItem()
                        {
                            Header = "Закрыть",
                            Command = new RelayCommand(param =>
                                Application.Current.Shutdown())
                        }
                    ]
                },
            new MenuItem()
                {
                    Header = "О нас",
                    Command = new RelayCommand(param =>
                        MessageBox.Show(
                            "Hello world",
                            "О нас",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information))
                }
        ];
}
