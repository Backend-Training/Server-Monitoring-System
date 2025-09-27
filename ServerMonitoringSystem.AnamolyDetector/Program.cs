using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using MongoDB.Driver;
using RabbitMQ;
using RabbitMQ.Configuration;
using ServerMonitoringSystem.AnamolyDetector.Models;
using ServerMonitoringSystem.AnamolyDetector.MongoDB;
using ServerMonitoringSystem.AnamolyDetector.Persitence;
using ServerMonitoringSystem.AnamolyDetector.Services;
using ServerMonitoringSystem.AnamolyDetector.Services.Interfaces;
using ServerMonitoringSystem.AnamolyDetector.Signaling.Interfaces;
using ServerMonitoringSystem.AnamolyDetector.Signaling.SignalR;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
var consumer = await RabbitMqConsumer.CreateAsync(
    new RabbitMqSettings(),
    exchangeName: "server_stats",
    queueName: "server_stats_queue",
    routingKey: "ServerStatistics.*"
);

IRepository repositoryService = new MongoDbRepository(
    databaseName: configuration["MongoDB:Database"],
    collectionName: configuration["MongoDB:DatabaseCollection"],
    client: new MongoClient(configuration["MongoDB:MongoURI"])
);

ISignal signalR = new SignalRAlerting(
    new HubConnectionBuilder()
        .WithUrl(configuration["SignalRConfig:SignalRUrl"])
        .WithAutomaticReconnect()
        .Build()
);
IAlert anamolyAlertService = new AnamolyAlertService(
    repository: repositoryService,
    signal: signalR,
    anomalyDetectionConfig: configuration["AnomalyDetectionConfig"]
);
IAlert highUsageAlertService = new HighUsageAlertService(
    signal: signalR,
    highUsageThersholdConfig: configuration["AnomalyDetectionConfig"]
);

await consumer.StartAsync(async (message, routingKey) =>
{
    var identifer = routingKey.Split(".")[1];
    var serverStatistics = JsonSerializer.Deserialize<ServerStatistics>(message);
    serverStatistics.ServerIdentifier = identifer;
    
    await anamolyAlertService.SendAlert(serverStatistics);
    await highUsageAlertService.SendAlert(serverStatistics);

    await repositoryService.Save(serverStatistics);
});