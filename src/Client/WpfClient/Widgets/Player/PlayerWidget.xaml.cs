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
    /// �������������� ����� ��������� �������� PlayerWidget, ����������� �������� ������, ��������� �������
    /// � �������� ��������� ���������� �� ������ � �������.
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
    /// ���������� ����������� ������ �� ��������� ������ ������ � ������� � ������������ ����� � �������������� callback-�������.
    /// </summary>
    private void ConnectToServer()
    {
        _playerGrpcClient.GetAsync(HandlePlayerGet, _cancellationTokenSource.Token);
    }

    /// <summary>
    /// ��������� ������ ������ �������, ����������� � �������.
    /// </summary>
    /// <param name="response">DTO (������ �������� ������) ������, ���������� ����������� ���������� �� ������.</param>
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
    /// ������������ ������� Unloaded ��������, ������� ��� ������������� ����������� ��������.
    /// </summary>
    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }

    /// <summary>
    /// ����������� ������ Google <c>Money</c> � �������� <c>double</c>, �������������� ����� �������� �����.
    /// </summary>
    /// <param name="money">������ <c>Money</c>, ���������� ������� (units) � ����-������� (nanos).</param>
    /// <returns>��������� �������� �������� � ���� <c>double</c>.</returns>
    private double ConvertGoogleMoney(Money money)
        => money.Units + money.Nanos / 1_000_000_000.0;
}
