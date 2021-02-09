using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbCommon;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;

namespace ConsoleQueryData
{
    public class QueryBsonDoc : ITransientDependency
    {
        private readonly ILogger<QueryBsonDoc> _logger;

        public QueryBsonDoc(ILogger<QueryBsonDoc> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync()
        {
            var wrapper = new MongoDbWrapper(MongoDbConsts.LocalMongoDbConnectionString);
            wrapper.GetDatabase(MongoDbConsts.MoviesDb.DatabaseName);
            var collection = wrapper.GetCollection(MongoDbConsts.MoviesDb.MoviesBsonCollection);
            var result = collection.Find(new BsonDocument());
            await result.ForEachAsync(d => _logger.LogDebug(d.ToJson()));
        }
    }
}