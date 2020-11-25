using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBLib;
using MongoDBLib.Models;
using NLog;
using System.Threading.Tasks;

namespace ReplaceDataTest
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
            var filter = builder.Eq("name", "new name");

            //find the ID of the godfather movie...
            var theGodfather = await collection.FindAsync(filter);
            var theGodfatherMovie = theGodfather.FirstOrDefault();

            Movie replacementMovie = new Movie
            {
                MovieId = theGodfatherMovie.MovieId,
                Name = "Mad Max: Fury Road",
                Year = 2015,
                Actors = new[]
                {
                    new Actor {Name = "Tom Hardy"},
                    new Actor {Name = "Charlize Theron"},
                },
                Director = "George Miller"
            };


            ReplaceOneResult result = await collection.ReplaceOneAsync(filter, replacementMovie);

            Logger.Debug(result.ToBsonDocument());
        }
    }
}
