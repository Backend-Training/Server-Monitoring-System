using ServerMonitoringSystem.Consumer.Signaling.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace ServerMonitoringSystem.Consumer.Signaling.SignalR;

public class SignalRConsumer : ISignalConsumer
{
    private HubConnection _hubConnection;

    public SignalRConsumer(HubConnection hubConnection)
    {
        _hubConnection = hubConnection;
    }

    public async Task Start()
    {
        await _hubConnection.StartAsync();
    }

    public async Task Stop()
    {
        await _hubConnection.StopAsync();
    }

    public void Register(string signal, Action<string> action)
    {
        _hubConnection.On(signal, action);
    }
}