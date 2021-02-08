using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using NLog;

namespace MongoDBLib
{
    public class MongoDBHelper
    {
        static ILogger Logger = LogManager.GetCurrentClassLogger();
        private string _connectionString;
        private string _databaseName;
        public IMongoClient Client { get; private set;}
        public IMongoDatabase Database { get; private set;}
        public MongoDBHelper(string databaseName, string connectionString = "mongodb://localhost:27017")
        {
            this._connectionString = connectionString;
            _databaseName = databaseName;
            Initialization();
        }

        private void Initialization()
        {
            Client = new MongoClient(_connectionString);
            Database = this.Client.GetDatabase(_databaseName);
        }

        public async Task DumpDatabases()
        {
            using var cursor = await Client.ListDatabaseNamesAsync();
            await cursor.ForEachAsync(x => Logger.Debug(x));
        }
        public async Task DumpCollections()
        {
            using var cursor = await Database.ListCollectionNamesAsync();
            await cursor.ForEachAsync(x => Logger.Debug(x));
        }

        public async Task DropDatabaseAsync(string databaseName) {
            await Client.DropDatabaseAsync(databaseName);
        }
    }
}
