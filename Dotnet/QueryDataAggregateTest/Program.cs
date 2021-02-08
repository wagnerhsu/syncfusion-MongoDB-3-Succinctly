using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBLib;
using MongoDBLib.Models;
using System;
using System.Threading.Tasks;

namespace QueryDataAggregateTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            BsonMapper.Map();
            MongoDBHelper helper = new MongoDBHelper("moviesDb");
            var db = helper.Database;
            var collName = "movies_bson";
            var collection = db.GetCollection<Movie>(collName);

            var aggregate = collection.Aggregate()
                .Match(Builders<Movie>.Filter.Where(x => x.Name.Contains("Sam")))
                .Group(new BsonDocument
                {
                    {"_id","$year" },
                    {"count",new BsonDocument("$sum",1) }
                });
            var results = aggregate.ToList();
            foreach (var item in results)
            {
                Console.WriteLine("Item retrieved {0}", item.ToString());
            }
            await Task.CompletedTask;
        }
    }
}