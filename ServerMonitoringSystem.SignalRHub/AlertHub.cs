using Microsoft.AspNetCore.SignalR;
namespace ServerMonitoringSystem.SignalRHub.AnomalyAlertsHubs;

public class AlertHub : Hub
{
    public async Task MemoryAnomaly(string message)
    {
        await Clients.All.SendAsync("ReceiveAlert", message);
    }

    public async Task CPUAnomaly(string message)
    {
        await Clients.All.SendAsync("ReceiveAlert",message);
    }
    
    public async Task MemoryHighUsage(string message)
    {
        await Clients.All.SendAsync("ReceiveAlert", message);
    }
    
    public async Task CPUHighUsage(string message)
    {
        await Clients.All.SendAsync("ReceiveAlert", message);
    }
}