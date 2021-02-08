using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBLib;
using MongoDBLib.Models;
using NLog;
using System.Threading.Tasks;

namespace QueryDataTest
{
    internal class Program
    {
        private static ILogger Logger = LogManager.GetCurrentClassLogger();

        private static async Task Main(string[] args)
        {
            MongoDBHelper helper = new MongoDBHelper("moviesDb");
            var db = helper.Database;
            var collName = "movies_bson";
            var collection = db.GetCollection<BsonDocument>(collName);
            var filter = new BsonDocument();
            int count = 0;
            using var cursor = await collection.FindAsync<BsonDocument>(filter);
            while (await cursor.MoveNextAsync())
            {
                var batch = cursor.Current;
                foreach (var document in batch)
                {
                    var movieName = document.GetElement("name").Value.ToString();
                    Logger.Debug("Movie Name: {0}", movieName);
                    count++;
                }
            }
            using var cursor1 = await collection.FindAsync<BsonDocument>(filter);
            await cursor1.ForEachAsync(x => Logger.Debug(x.GetElement("name").Value.AsString));
            
            
        }
     
    }
}