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
            MongoDbWrapper wrapper = new MongoDbWrapper("mongodb://localhost:27017");
            wrapper.GetDatabase("foo");
            var collection = wrapper.GetCollection("bar");
            var result = collection.Find(new BsonDocument());
            await result.ForEachAsync(d => _logger.LogDebug(JsonConvert.SerializeObject(result)));
        }
    }
}