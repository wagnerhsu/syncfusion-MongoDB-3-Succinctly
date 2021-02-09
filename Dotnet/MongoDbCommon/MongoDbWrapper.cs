using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbCommon
{
    public class MongoDbWrapper
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _database;
        public MongoDbWrapper(string connectionString)
        {
            _mongoClient = new MongoClient(connectionString);
        }

        public IMongoDatabase GetDatabase(string name)
        {
            _database = _mongoClient.GetDatabase(name);
            return _database;
        }

        public IMongoCollection<BsonDocument> GetCollection(string name)
        {
            return _database.GetCollection<BsonDocument>(name);
        }
    }
}