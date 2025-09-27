using ServerMonitoringSystem.AnamolyDetector.Models;
using ServerMonitoringSystem.AnamolyDetector.Services.Interfaces;
using ServerMonitoringSystem.AnamolyDetector.Signaling.Interfaces;
using ServerMonitoringSystem.AnamolyDetector.Utils;

namespace ServerMonitoringSystem.AnamolyDetector.Services;

public class HighUsageAlertService : IAlert
{
    private ISignal _signal;
    private ServerStatisticsThershold _serverStatisticsTherhshold;

    public HighUsageAlertService(ISignal signal, string highUsageThersholdConfig)
    {
        _signal = signal;
        _serverStatisticsTherhshold = this.GetThersholdDetectionConfig(highUsageThersholdConfig);
    }

    public async Task SendAlert(ServerStatistics currentStatistics)
    {
        await _signal.Start();

        if (currentStatistics.MemoryUsage / (currentStatistics.MemoryUsage + currentStatistics.AvailableMemory) >
            _serverStatisticsTherhshold.MemoryUsageThresholdPercentage)
        {
            await _signal.Invoke("MemoryHighUsage", "Memory High Usage Alert");
        }

        if (currentStatistics.CpuUsage > _serverStatisticsTherhshold.CpuUsageThresholdPercentage)
        {
            await _signal.Invoke("CPUHighUsage", "CPU High Usage Alert");
        }
        
        await _signal.Stop();
    }
}