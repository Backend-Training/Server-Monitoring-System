using System.Diagnostics;
using Hardware.Info;
using ServerMonitoringSystem.Collector.DTOs;
namespace ServerMonitoringSystem.Collector.Services;


public class ServerStatisticsService
{
    
    private readonly IHardwareInfo _hardwareInfo;

    public ServerStatisticsService()
    {
        _hardwareInfo = new HardwareInfo();
    }

    public ServerStatistics Collect()
    {
        _hardwareInfo.RefreshMemoryStatus();
        _hardwareInfo.RefreshCPUList();
        
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
        return _hardwareInfo.MemoryStatus.TotalPhysical / (1024.0 * 1024.0);
    }

    private double GetAvailableMemory()
    {
        return _hardwareInfo.MemoryStatus.AvailablePhysical / (1024.0 * 1024.0);
    }

    private double GetCpuUsage()
    {
        double cpuUsage = 0;
        foreach (var cpu in _hardwareInfo.CpuList)
        {
            cpuUsage += cpu.PercentProcessorTime;
        }
        cpuUsage /= _hardwareInfo.CpuList.Count;
        return cpuUsage;
    }

    private DateTime GetTimeStamp()
    {
        return DateTime.Now;
    }
}