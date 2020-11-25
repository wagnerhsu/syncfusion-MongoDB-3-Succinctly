using MongoDBLib;
using MongoDBLib.Models;
using System;
using System.Threading.Tasks;

namespace InsertDataWithPoco
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var movies = MovieManager.GetMovieList();
            BsonMapper.Map();
            MongoDBHelper helper = new MongoDBHelper("moviesDb");
            var moviesCollection = helper.Database.GetCollection<Movie>("movies_poco");
            await moviesCollection.InsertManyAsync(movies);
        }
    }
}
