using MongoDB.Bson;
using MongoDBLib;
using System;
using System.Threading.Tasks;

namespace InsertDataTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MongoDBHelper helper = new MongoDBHelper("moviesDb");
            var moviesCollection = helper.Database.GetCollection<BsonDocument>("movies_bson");
            await moviesCollection.InsertManyAsync(MovieManager.GetBsonMovies());

        }
    }
}
