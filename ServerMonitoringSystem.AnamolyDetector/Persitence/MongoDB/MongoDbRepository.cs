using MongoDB.Driver;
using ServerMonitoringSystem.AnamolyDetector.Models;
using ServerMonitoringSystem.AnamolyDetector.Persitence;

namespace ServerMonitoringSystem.AnamolyDetector.MongoDB;

public class MongoDbRepository : IRepository
{
   
    private IMongoDatabase _database;
    private IMongoCollection<ServerStatistics> _collection;
    private IMongoClient _client;

    public MongoDbRepository(string databaseName, string collectionName, IMongoClient client)
    {
        _client = client;
        _database = _client.GetDatabase(databaseName);;
        _collection = _database.GetCollection<ServerStatistics>(collectionName);
    }

    public async Task Save(ServerStatistics serverStatistics)
    {
         await _collection.InsertOneAsync(serverStatistics);
    }

    public ServerStatistics GetLastItem()
    {
        return _collection
            .Find(Builders<ServerStatistics>.Filter.Empty)
            .SortByDescending(x => x.Timestamp)
            .FirstOrDefault();
    }
}