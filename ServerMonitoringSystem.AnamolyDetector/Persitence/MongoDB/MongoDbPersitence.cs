using MongoDB.Driver;
using ServerMonitoringSystem.AnamolyDetector.Models;
using ServerMonitoringSystem.AnamolyDetector.Persitence;

namespace ServerMonitoringSystem.AnamolyDetector.MongoDB;

public class MongoDbPersitence : IPersitence
{
    private string _connectionString;
    private string _databaseName;
    private string _collectionName;
    private IMongoClient _client;

    public MongoDbPersitence(
        string connectionString, 
        string databaseName, 
        string collectionName, 
        IMongoClient client)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
        _collectionName = collectionName;
        _client = client;
    }

    public async Task Save(ServerStatistics serverStatistics)
    {
        var database = _client.GetDatabase(_databaseName);
        var collection = database.GetCollection<ServerStatistics>(_collectionName);

        await collection.InsertOneAsync(serverStatistics);
    }
}