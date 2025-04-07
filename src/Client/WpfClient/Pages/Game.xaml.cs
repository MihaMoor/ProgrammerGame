using System.Windows.Controls;
using WpfClient.Widgets;
using WpfClient.Widgets.MainStats;

namespace WpfClient.Pages;

/// <summary>
/// Interaction logic for Game.xaml
/// </summary>
public partial class Game : Page
{
    public Game(MenuBar menuBar, MainStats mainStats)
    {
        InitializeComponent();
        MenuBarFrame.Navigate(menuBar);
        PlayerStatsFrame.Navigate(mainStats);
    }
}
