using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Configuration;
using ServerMonitoringSystem.Collector.Configuration;
using ServerMonitoringSystem.Collector.Services;

await Task.Delay(1000);
Console.WriteLine("Welcome From Collection Service");
await Task.Delay(1000);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();


var serverStatisticsService = new ServerStatisticsService();


var publisher = await RabbitMqPublisher.CreateAsync(
    new RabbitMqSettings(), // I Used Default Settings Located In This Class
    "server_stats",
    $"ServerStatistics.{configuration["ServerStatisticsConfig:ServerIdentifier"]}"
);



// Keep Sending Every SamplingInterval Seconds
while (true)
{
    var serverStatistics = serverStatisticsService.Collect();

    await publisher.PublishAsync(JsonSerializer.Serialize(serverStatistics));
    
    Console.WriteLine($"Published stats for {configuration["ServerStatisticsConfig:ServerIdentifier"]} at {serverStatistics.Timestamp}");
    
    await Task.Delay(TimeSpan.FromSeconds(Double.Parse(configuration["ServerStatisticsConfig:SamplingIntervalSeconds"])));
}