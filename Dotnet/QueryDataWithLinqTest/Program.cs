using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDBLib;
using MongoDBLib.Models;
using NLog;
using System.Threading.Tasks;

namespace QueryDataWithLinqTest
{
    internal class Program
    {
        private static ILogger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            BsonMapper.Map();
            MongoDBHelper helper = new MongoDBHelper("moviesDb");
            var db = helper.Database;
            var collName = "movies_bson";
            var collection = db.GetCollection<Movie>(collName);

            var movies = collection.AsQueryable()
                .Where(x => x.Name == "The Godfather")
                .Select(x => new { MovieName = x.Name, MainActor = x.Actors[0].Name });
            foreach (var movie in movies)
            {
                Logger.Debug("Movie name: {0} and Main Actor: {1}", movie.MovieName, movie.MainActor);
            }

            Task.Run(async () =>
            {
                var results = await collection.AsQueryable()
                    .Where(x => x.Name == "The Godfather")
                    .Select(x => new { MovieName = x.Name, MainActor = x.Actors[0].Name })
                    .ToListAsync();
                foreach (var item in results)
                {
                    Logger.Debug(item.ToString());
                }
            }).Wait();
        }
    }
}