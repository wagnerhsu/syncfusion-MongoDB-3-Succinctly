using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBLib;
using MongoDBLib.Models;
using NLog;
using System.Threading.Tasks;

namespace DeleteDataTest
{
    class Program
    {
        private static ILogger Logger = LogManager.GetCurrentClassLogger();

        private static async Task Main(string[] args)
        {
            BsonMapper.Map();
            MongoDBHelper helper = new MongoDBHelper("moviesDb");
            var db = helper.Database;
            var collName = "movies_bson";
            var collection = db.GetCollection<Movie>(collName);

            var builder = Builders<Movie>.Filter;
            var filter = builder.Eq("name", "Mad Max: Fury Road");

            //DeleteResult result = await collection.DeleteOneAsync(m => m.Name == "The Seven Samurai");
            //var result = await collection.DeleteManyAsync(m => m.Name == "The Seven Samurai" || m.Name == "Cabaret");

            //var builder = Builders<Movie>.Filter;
            //var filter = builder.Eq("name", "The Godfather");
            //var result = await collection.DeleteManyAsync(filter);

            BsonDocument result =
                await
                    collection.FindOneAndDeleteAsync(m => m.Name == "Mad Max: Fury Road",
                        new FindOneAndDeleteOptions<Movie, BsonDocument>
                        {
                            Sort = Builders<Movie>.Sort.Ascending(x => x.Name),
                            Projection = Builders<Movie>.Projection.Include(x => x.MovieId)
                        });

            Logger.Debug(result.ToBsonDocument());
        }
    }
}
