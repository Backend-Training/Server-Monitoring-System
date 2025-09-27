
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using ServerMonitoringSystem.Consumer.Signaling.Interfaces;
using ServerMonitoringSystem.Consumer.Signaling.SignalR;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();


ISignalConsumer consumer = new SignalRConsumer(
    new HubConnectionBuilder()
        .WithUrl(configuration["SignalRConfig:SignalRUrl"])
        .WithAutomaticReconnect()
        .Build()
);  

await consumer.Start();

consumer.Register("ReceiveAlert", (message) =>
{
    Console.WriteLine($"{DateTime.Now:HH:mm:ss}  {message}");
});

await Task.Delay(-1); // Keep it alive