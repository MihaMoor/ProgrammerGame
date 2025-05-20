using System.Windows.Controls;
using WpfClient.Widgets;

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
