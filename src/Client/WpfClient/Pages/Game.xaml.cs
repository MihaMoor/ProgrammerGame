using System.Windows.Controls;
using WpfClient.Widgets.MenuBar;
using WpfClient.Widgets.PlayerWidget;

namespace WpfClient.Pages;

/// <summary>
/// Interaction logic for Game.xaml
/// </summary>
public partial class Game : Page
{
    public Game(MenuBar menuBar, PlayerWidget mainStats)
    {
        InitializeComponent();
        MenuBarFrame.Navigate(menuBar);
        PlayerStatsFrame.Navigate(mainStats);
    }
}
