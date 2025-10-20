using ServerMonitoringSystem.AnamolyDetector.Models;
using ServerMonitoringSystem.AnamolyDetector.Services.Interfaces;
using ServerMonitoringSystem.AnamolyDetector.Signaling.Interfaces;

namespace ServerMonitoringSystem.AnamolyDetector.Services;

public class HighUsageAlertService : IAlert
{
    private ISignal _signal;
    private ServerStatisticsThershold _serverStatisticsTherhshold;

    public HighUsageAlertService(ISignal signal, ServerStatisticsThershold serverStatisticsTherhshold)
    {
        _signal = signal;
        _serverStatisticsTherhshold = serverStatisticsTherhshold;
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