using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbCommon;
using MongoDBLib;
using MongoDBLib.Models;
using Volo.Abp.DependencyInjection;

namespace ConsoleDeleteData
{
    public class DeleteDataWithBsonMapper : ITransientDependency
    {
        private readonly ILogger<DeleteDataWithBsonMapper> _logger;

        public DeleteDataWithBsonMapper(ILogger<DeleteDataWithBsonMapper> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync()
        {
            BsonMapper.Map();
            var wrapper = new MongoDbWrapper(MongoDbConsts.LocalMongoDbConnectionString);
            wrapper.GetDatabase(MongoDbConsts.MoviesDb.DatabaseName);
            var moviesCollection = wrapper.GetCollection<Movie>(MongoDbConsts.MoviesDb.MoviesBsonCollection);
            var builder = Builders<Movie>.Filter;
            var filter = builder.Eq("name", "The Godfather");
            var deleteResult = await moviesCollection.DeleteManyAsync(filter);
            _logger.LogDebug(deleteResult.ToJson());
            BsonDocument result =
                await
                    moviesCollection.FindOneAndDeleteAsync(m => m.Name == "Mad Max: Fury Road",
                        new FindOneAndDeleteOptions<Movie, BsonDocument>
                        {
                            Sort = Builders<Movie>.Sort.Ascending(x => x.Name),
                            Projection = Builders<Movie>.Projection.Include(x => x.MovieId)
                        });
            _logger.LogDebug(result?.ToBsonDocument().ToJson());
        }
    }
}