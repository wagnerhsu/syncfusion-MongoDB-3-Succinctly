using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDbCommon;
using MongoDBLib;
using MongoDBLib.Models;
using Volo.Abp.DependencyInjection;

namespace ConsoleQueryData
{
    public class QueryWithLinq: ITransientDependency
    {
        private readonly ILogger<QueryWithLinq> _logger;

        public QueryWithLinq(ILogger<QueryWithLinq> logger)
        {
            _logger = logger;
        }
        public async Task RunAsync()
        {
            BsonMapper.Map();
            var wrapper = new MongoDbWrapper(MongoDbConsts.LocalMongoDbConnectionString);
            wrapper.GetDatabase(MongoDbConsts.MoviesDb.DatabaseName);
            var collection = wrapper.GetCollection<Movie>(MongoDbConsts.MoviesDb.MoviesBsonCollection);
            var results = await collection.AsQueryable()
                .Where(x => x.Name == "The Godfather")
                .Select(x => new { MovieName = x.Name, MainActor = x.Actors[0].Name })
                .ToListAsync();
            foreach (var item in results)
            {
                _logger.LogDebug(item.ToJson());
            }
        }
    }
}