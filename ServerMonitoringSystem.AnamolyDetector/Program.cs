using Microsoft.Extensions.Configuration;
using System.Text.Json;
using MongoDB.Driver;
using RabbitMQ;
using RabbitMQ.Configuration;
using ServerMonitoringSystem.AnamolyDetector.Models;
using ServerMonitoringSystem.AnamolyDetector.MongoDB;
using ServerMonitoringSystem.AnamolyDetector.Persitence;

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

IPersitence persitenceService = new MongoDbPersitence(
    databaseName:configuration["MongoDB:Database"],
    collectionName:configuration["MongoDB:DatabaseCollection"],
    client: new MongoClient(configuration["MongoDB:MongoURI"])
);

await consumer.StartAsync(async (message, routingKey) =>
{
    var identifer = routingKey.Split(".")[1];
    var serverStatistics = JsonSerializer.Deserialize<ServerStatistics>(message);
    serverStatistics.ServerIdentifier = identifer;
    
    await persitenceService.Save(serverStatistics);
});
