using ServerMonitoringSystem.AnamolyDetector.Models;

namespace ServerMonitoringSystem.AnamolyDetector.Persitence;

public interface IRepository
{
    public Task Save(ServerStatistics serverStatistics);
    public ServerStatistics GetLastItem();
}