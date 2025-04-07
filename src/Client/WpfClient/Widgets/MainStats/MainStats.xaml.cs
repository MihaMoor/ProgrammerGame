using Client.Infrastructure.Clients;
using System.Windows.Controls;

namespace WpfClient.Widgets.MainStats;

/// <summary>
/// Interaction logic for MainStats.xaml
/// </summary>
public partial class MainStats : Page
{
    private readonly PlayerMainStatsGrpcClient _grpcClient;

    public MainStats(PlayerMainStatsGrpcClient grpcClient)
    {
        _grpcClient = grpcClient;
        InitializeComponent();
        Init();
    }

    private async void Init()
    {
        Shared.GrpcContracts.PlayerMainStatsDto responce = await _grpcClient.GetAsync();

        this.DataContext = new PlayerMainStats(
            health: responce.Health,
            hunger: responce.Hunger,
            money: responce.Money,
            mood: responce.Mood
        );
    }
}
