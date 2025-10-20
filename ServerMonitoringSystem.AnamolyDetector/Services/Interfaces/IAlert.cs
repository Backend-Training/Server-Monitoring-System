using ServerMonitoringSystem.AnamolyDetector.Models;

namespace ServerMonitoringSystem.AnamolyDetector.Services.Interfaces;

public interface IAlert
{
    public Task SendAlert(ServerStatistics currentStatistics);
}