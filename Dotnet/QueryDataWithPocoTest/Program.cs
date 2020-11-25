using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBLib;
using MongoDBLib.Models;
using NLog;
using System;
using System.Threading.Tasks;

namespace QueryDataWithPocoTest
{
    class Program
    {
        private static ILogger Logger = LogManager.GetCurrentClassLogger();
        static async Task Main(string[] args)
        {
            BsonMapper.Map();
            MongoDBHelper helper = new MongoDBHelper("moviesDb");
            var db = helper.Database;
            var collName = "movies_bson";
            var collection = db.GetCollection<Movie>(collName);
            var filter = new BsonDocument();
            int count = 0;
            using (var cursor = await collection.FindAsync<Movie>(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var movie in batch)
                    {

                        Console.WriteLine("Movie Name: {0}", movie.Name);

                        count++;
                    }
                }
            }
            await FindMoviesByName(collection, "The Godfather");
            await FindMoviesWithProjectionsAsync(collection);
        }
        static async Task FindMoviesByName(IMongoCollection<Movie> collection, string movieName)
        {
            var filter = Builders<Movie>.Filter.Eq(x => x.Name, movieName);
            var sort = Builders<Movie>.Sort.Ascending(x => x.Name).Descending(x => x.Year);

            var result = await collection
                .Find(filter)
                .Sort(sort)
                .ToListAsync();
            foreach (var movie in result)
            {
                Logger.Debug("Match found: movie with name '{0}' exists", movie.Name);
            }
        }
        static async Task FindMoviesWithProjectionsAsync(IMongoCollection<Movie> collection)
        {
            var projection = Builders<Movie>.Projection
                .Include("name")
                .Include("year")
                .Exclude("_id");

            using var cursor = await collection.FindAsync(new BsonDocument(), new FindOptions<Movie, BsonDocument> { Projection = projection });
            await cursor.ForEachAsync(x => Logger.Debug(x.ToString()));
        }
    }
}
