using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbCommon;
using MongoDBLib.Models;
using Volo.Abp.DependencyInjection;

namespace ConsoleUpdateData
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

            _logger.LogDebug(result.ToBsonDocument().ToJson());

            await ReplaceData(collection);
        }

        private async Task ReplaceData(IMongoCollection<Movie> collection)
        {
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

            _logger.LogDebug(result.ToBsonDocument().ToJson());
        }
    }
}
