using System.Text.Json;
using RabbitMQ.Configuration;
using ServerMonitoringSystem.Collector.Configuration;
using ServerMonitoringSystem.Collector.Services;

var jsonFile = File.ReadAllText("appsettings.json");
var rootConfig = JsonSerializer.Deserialize<RootConfig>(jsonFile);
var serverSettings = rootConfig.ServerStatisticsConfig;

var serverStatisticsService = new ServerStatisticsService();


var publisher = await RabbitMqPublisher.CreateAsync(
    new RabbitMqSettings(), // I Used Default Settings Located In This Class
    "server_stats",
    $"ServerStatistics.{serverSettings.ServerIdentifier}"
);

// Keep Sending Every SamplingInterval Seconds
while (true)
{
    var serverStatistics = serverStatisticsService.Collect();

    await publisher.PublishAsync(JsonSerializer.Serialize(serverStatistics));
    
    Console.WriteLine($"Published stats for {serverSettings.ServerIdentifier} at {serverStatistics.Timestamp}");
    
    await Task.Delay(TimeSpan.FromSeconds(serverSettings.SamplingIntervalSeconds));
}