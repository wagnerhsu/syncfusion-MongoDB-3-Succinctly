using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDbCommon;
using Volo.Abp.DependencyInjection;

namespace ConsoleInsertData
{
    public class InsertBsonDoc : ITransientDependency
    {
        public async Task RunAsync()
        {
            var wrapper = new MongoDbWrapper(MongoDbConsts.LocalMongoDbConnectionString);
            wrapper.GetDatabase("foo");
            var collection = wrapper.GetCollection("bar");
            collection.InsertOneAsync(new BsonDocument
            {
                {"name", "MongoDB"},
                {"type", "Database"},
                {"count", 1},
                {
                    "info", new BsonDocument
                    {
                        {"x", 203},
                        {"y", 102}
                    }
                }
            });

            await Task.CompletedTask;
        }
    }
}