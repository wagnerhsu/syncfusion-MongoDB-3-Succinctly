using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBLib;
using MongoDBLib.Models;
using NLog;
using System.Threading.Tasks;

namespace UpdateDataTest
{
    internal class Program
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
            var filter = builder.Eq("name", "The Godfather");
            var update = Builders<Movie>.Update
                .Set("name", "new name")
                .Set("newProperty", "something goes here")
                .Set(d => d.Year, 1900);

            var updateOptions = new FindOneAndUpdateOptions<Movie, Movie>()
            {
                ReturnDocument = ReturnDocument.After,
                Projection = Builders<Movie>
                    .Projection
                    .Include(x => x.Year)
                    .Include(x => x.Name),
            };

            //-- method 1 FindOneAndUpdate
            Movie result = await collection.FindOneAndUpdateAsync(filter, update, updateOptions);

            //-- method 2 Update One
            //UpdateResult result = collection.UpdateOne(filter, update);

            //-- method 3 UpdateMany
            //UpdateResult result = collection.UpdateMany(filter, update);

            Logger.Debug(result.ToBsonDocument());
        }
    }
}