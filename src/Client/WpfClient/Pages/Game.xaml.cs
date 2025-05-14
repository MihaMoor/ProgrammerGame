using System.Windows.Controls;
using WpfClient.Widgets;

namespace WpfClient.Pages;

/// <summary>
/// Interaction logic for Game.xaml
/// </summary>
public partial class Game : Page
{
    /// <summary>
    /// Initializes the Game page and navigates its frames to display the provided menu bar and main stats widgets.
    /// </summary>
    /// <param name="menuBar">The menu bar widget to display in the MenuBarFrame.</param>
    /// <param name="mainStats">The main stats widget to display in the PlayerStatsFrame.</param>
    public Game(MenuBar menuBar, MainStatsWidget mainStats)
    {
        InitializeComponent();
        MenuBarFrame.Navigate(menuBar);
        PlayerStatsFrame.Navigate(mainStats);
    }
}
