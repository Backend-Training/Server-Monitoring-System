using System.Text.Json;
using ServerMonitoringSystem.AnamolyDetector.Models;
using ServerMonitoringSystem.AnamolyDetector.Utils;
using ServerMonitoringSystem.AnamolyDetector.Persitence;
using ServerMonitoringSystem.AnamolyDetector.Services.Interfaces;
using ServerMonitoringSystem.AnamolyDetector.Signaling.Interfaces;

namespace ServerMonitoringSystem.AnamolyDetector.Services;

public class AnamolyAlertService : IAlert
{
    private IRepository _repository;
    private ISignal _signal;
    private ServerStatisticsThershold _serverStatisticsTherhshold;

    public AnamolyAlertService(IRepository repository, ISignal signal, string anomalyDetectionConfig)
    {
        _repository = repository;
        _signal = signal;
        _serverStatisticsTherhshold = this.GetThersholdDetectionConfig(anomalyDetectionConfig);
    }

    public async Task SendAlert(ServerStatistics currentStatistics)
    {
        var prevStatistcs = _repository.GetLastItem();
        if (currentStatistics.MemoryUsage >
            prevStatistcs.MemoryUsage * (1 + _serverStatisticsTherhshold.MemoryUsageAnomalyThresholdPercentage))
        {
            _signal.Invoke("MemoryAnomaly", "Memory Usage Anomaly Alert");
        }

        if (currentStatistics.CpuUsage >
            prevStatistcs.CpuUsage * (1 + _serverStatisticsTherhshold.CpuUsageAnomalyThresholdPercentage))
        {
            _signal.Invoke("CPUAnomaly", "CPU Usage Anomaly Alert");
        }
    }
}