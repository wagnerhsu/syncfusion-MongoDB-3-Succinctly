using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDbCommon;
using Volo.Abp.DependencyInjection;

namespace ConsoleInsertData
{
    public class InsertBsonDoc: ITransientDependency
    {
        public async Task RunAsync()
        {
            MongoDbWrapper wrapper = new MongoDbWrapper("mongodb://localhost:27017");
            wrapper.GetDatabase("foo");
            var collection = wrapper.GetCollection("bar");
            collection.InsertOneAsync(new BsonDocument
            {
                { "name", "MongoDB" },
                { "type", "Database" },
                { "count", 1 },
                { "info", new BsonDocument
                {
                    { "x", 203 },
                    { "y", 102 }
                }}
            });
                
            await Task.CompletedTask;
        }
    }
}