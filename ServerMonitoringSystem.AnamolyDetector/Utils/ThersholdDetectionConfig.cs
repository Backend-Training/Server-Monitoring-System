using System.Text.Json;
using ServerMonitoringSystem.AnamolyDetector.Models;
using ServerMonitoringSystem.AnamolyDetector.Services.Interfaces;

namespace ServerMonitoringSystem.AnamolyDetector.Utils;

public static class ThersholdDetectionConfig
{
    public static ServerStatisticsThershold GetThersholdDetectionConfig(this IAlert alert, string config)
    {
        return JsonSerializer.Deserialize<ServerStatisticsThershold>(config);
    }
}