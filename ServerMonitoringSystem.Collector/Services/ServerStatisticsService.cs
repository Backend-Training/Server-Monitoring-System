using System.Diagnostics;
using ServerMonitoringSystem.Collector.DTOs;
namespace ServerMonitoringSystem.Collector.Services;


public class ServerStatisticsService
{
    
    private PerformanceCounter _cpuMonitor;
    private PerformanceCounter _memoryMonitor;

    public ServerStatisticsService()
    {
        _cpuMonitor = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        _memoryMonitor = new PerformanceCounter("Memory", "Available MBytes");
        
        _cpuMonitor.NextValue();
        _memoryMonitor.NextValue();
        
    }
    public ServerStatistics Collect()
    {
        // Warm Up Time For Getting Right Results
        Thread.Sleep(500);
        
        var availableMemory = GetAvailableMemory();
        var cpuUsage = GetCpuUsage();
        var totalMemory = GetTotalMemory();
        var memoryUsage = (totalMemory - availableMemory) / totalMemory;
        
        return new ServerStatistics()
        {
            MemoryUsage = memoryUsage,
            AvailableMemory = availableMemory,
            CpuUsage = cpuUsage,
            Timestamp = GetTimeStamp()
        };
    }
    private double GetTotalMemory()
    {
        return (double)Environment.WorkingSet / (1024.0 * 1024.0); // Convert To Mb
    }

    private double GetAvailableMemory()
    {
        return _memoryMonitor.NextValue();
    }

    private double GetCpuUsage()
    {
        return _cpuMonitor.NextValue();
    }

    private DateTime GetTimeStamp()
    {
        return DateTime.Now;
    }
}