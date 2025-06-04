using System.Windows;
using System.Windows.Controls;
using Client.Infrastructure.Clients;
using Google.Type;
using Server.Module.Player.GrpcContracts.V1;

namespace WpfClient.Widgets;

/// <summary>
/// Interaction logic for .xaml
/// </summary>
public partial class PlayerWidget : Page
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Player _player;
    private readonly PlayerGrpcClient _playerGrpcClient;

    /// <summary>
    /// »нициализирует новый экземпл€р страницы PlayerWidget, настраивает прив€зку данных, обработку событий
    /// и начинает получение информации об игроке с сервера.
    /// </summary>
    public PlayerWidget(PlayerGrpcClient grpcClient)
    {
        _player = new();
        _playerGrpcClient = grpcClient;
        _cancellationTokenSource = new();
        InitializeComponent();
        DataContext = _player;
        Unloaded += PageUnloaded;
        ConnectToServer();
    }

    /// <summary>
    /// »нициирует асинхронный запрос на получение данных игрока с сервера и обрабатывает ответ с использованием callback-функции.
    /// </summary>
    private void ConnectToServer()
    {
        _playerGrpcClient.GetAsync(HandlePlayerGet, _cancellationTokenSource.Token);
    }

    /// <summary>
    /// ќбновл€ет модель игрока данными, полученными с сервера.
    /// </summary>
    /// <param name="response">DTO (объект передачи данных) игрока, содержащий обновленную информацию об игроке.</param>
    private void HandlePlayerGet(PlayerDto response)
    {
        Dispatcher.Invoke(() =>
        {
            _player.Name = response.Name;
            _player.Health = response.Health;
            _player.Hunger = response.Hunger;
            _player.Mood = response.Mood;
            _player.PocketMoney = ConvertGoogleMoney(response.PocketMoney);
        });
    }

    /// <summary>
    /// ќбрабатывает событие Unloaded страницы, отмен€€ все выполн€ющиес€ асинхронные операции.
    /// </summary>
    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }

    /// <summary>
    /// ѕреобразует объект Google <c>Money</c> в значение <c>double</c>, представл€ющее общую денежную сумму.
    /// </summary>
    /// <param name="money">ќбъект <c>Money</c>, содержащий единицы (units) и нано-единицы (nanos).</param>
    /// <returns>—уммарное денежное значение в виде <c>double</c>.</returns>
    private double ConvertGoogleMoney(Money money)
        => money.Units + money.Nanos / 1_000_000_000.0;
}
