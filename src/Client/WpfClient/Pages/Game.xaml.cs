using System.Windows.Controls;
using WpfClient.Widgets;

namespace WpfClient.Pages;

/// <summary>
/// Interaction logic for Game.xaml
/// </summary>
public partial class Game : Page
{
    /// <summary>
    /// Initializes the Game page with the specified menu bar and main stats widgets.
    /// </summary>
    /// <param name="menuBar">The menu bar control to display in the page's menu bar frame.</param>
    /// <param name="mainStats">The main stats widget to display in the player stats frame.</param>
    public Game(MenuBar menuBar, MainStatsWidget mainStats)
    {
        InitializeComponent();
        MenuBarFrame.Navigate(menuBar);
        PlayerStatsFrame.Navigate(mainStats);
    }
}
