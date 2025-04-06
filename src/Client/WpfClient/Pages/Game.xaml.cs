using System.Windows.Controls;
using WpfClient.Widgets;
using WpfClient.Widgets.Player;

namespace WpfClient.Pages;

/// <summary>
/// Interaction logic for Game.xaml
/// </summary>
public partial class Game : Page
{
    public Game()
    {
        InitializeComponent();
        MenuBarFrame.Navigate(new MenuBar());
        PlayerStatsFrame.Navigate(new MainStats());
    }
}
