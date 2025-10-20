using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using ServerMonitoringSystem.AnamolyDetector.Models;
using ServerMonitoringSystem.AnamolyDetector.Signaling.Interfaces;

namespace ServerMonitoringSystem.AnamolyDetector.Signaling.SignalR;

public class SignalRAlerting : ISignal
{
    private HubConnection _hubConnection;

    public SignalRAlerting(HubConnection hubConnection)
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

    public async Task Invoke(string signal, string message)
    {
        await _hubConnection.InvokeAsync(signal, message);
    }
}