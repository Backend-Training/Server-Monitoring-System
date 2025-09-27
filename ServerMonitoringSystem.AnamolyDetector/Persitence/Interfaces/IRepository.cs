using ServerMonitoringSystem.AnamolyDetector.Models;

namespace ServerMonitoringSystem.AnamolyDetector.Persitence;

public interface IPersitence
{
    public Task Save(ServerStatistics serverStatistics);
}