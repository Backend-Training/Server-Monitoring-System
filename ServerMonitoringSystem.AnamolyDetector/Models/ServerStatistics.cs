using System.Text.Json.Serialization;

namespace ServerMonitoringSystem.AnamolyDetector.Models;

public class ServerStatistics{

    [JsonIgnore] 
    public String ServerIdentifier { get; set; }
    public double MemoryUsage { get; set; } 
    public double AvailableMemory { get; set; }
    public double CpuUsage { get; set; }
    public DateTime Timestamp { get; set; }
}