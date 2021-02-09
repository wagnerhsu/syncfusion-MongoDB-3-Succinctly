using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbCommon;
using MongoDBLib.Models;
using Volo.Abp.DependencyInjection;

namespace ConsoleQueryWithCoco
{
    public class HelloWorldService : ITransientDependency
    {
        private readonly ILogger<HelloWorldService> _logger;

        public HelloWorldService(ILogger<HelloWorldService> logger)
        {
            _logger = logger;
        }
        public async Task RunAsync()
        {
            var wrapper = new MongoDbWrapper(MongoDbConsts.LocalMongoDbConnectionString);
            wrapper.GetDatabase(MongoDbConsts.MoviesDb.DatabaseName);
            var collection = wrapper.GetCollection<Movie>(MongoDbConsts.MoviesDb.MoviesBsonCollection);
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
            await FindMoviesWithProjectionsAsync(collection,"The Godfather");
            await AggregateTest(collection);
        }

        private async Task AggregateTest(IMongoCollection<Movie> collection)
        {
            var aggregate = collection.Aggregate()
                .Match(Builders<Movie>.Filter.Where(x => x.Name.Contains("Sam")))
                .Group(new BsonDocument
                {
                    {"_id","$year" },
                    {"count",new BsonDocument("$sum",1) }
                });
            var results = await aggregate.ToListAsync();
            foreach (var item in results)
            {
                Console.WriteLine("Item retrieved {0}", item.ToString());
            }
        }

        private async Task FindMoviesWithProjectionsAsync(IMongoCollection<Movie> collection, string movieName)
        {
            var filter = Builders<Movie>.Filter.Eq(x => x.Name, movieName);
            var sort = Builders<Movie>.Sort.Ascending(x => x.Name).Descending(x => x.Year);

            var result = await collection
                .Find(filter)
                .Sort(sort)
                .ToListAsync();
            foreach (var movie in result)
            {
                _logger.LogDebug("Match found: movie with name '{0}' exists", movie.Name);
            }
        }

        private async Task FindMoviesByName(IMongoCollection<Movie> collection, string theGodfather)
        {
            var projection = Builders<Movie>.Projection
                .Include("name")
                .Include("year")
                .Exclude("_id");

            using var cursor = await collection.FindAsync(new BsonDocument(), new FindOptions<Movie, BsonDocument> { Projection = projection });
            await cursor.ForEachAsync(x => _logger.LogDebug(x.ToString()));
        }
    }
}
