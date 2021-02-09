using System.Threading.Tasks;
using MongoDbCommon;
using MongoDBLib;
using Volo.Abp.DependencyInjection;

namespace ConsoleInsertData
{
    public class InsertMovieDb: ITransientDependency
    {
        public async Task RunAsync()
        {
            var wrapper = new MongoDbWrapper(MongoDbConsts.LocalMongoDbConnectionString);
            wrapper.GetDatabase(MongoDbConsts.MoviesDb.DatabaseName);
            var moviesCollection = wrapper.GetCollection(MongoDbConsts.MoviesDb.MoviesBsonCollection);
            await moviesCollection.InsertManyAsync(MovieManager.GetBsonMovies());
        }
    }
}