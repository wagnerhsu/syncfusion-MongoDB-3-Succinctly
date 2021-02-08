using MongoDBLib;
using System.Threading.Tasks;

namespace MongoDBTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            MongoDBHelper helper = new MongoDBHelper("devicedata");
            await helper.DumpCollections();
            await helper.DumpDatabases();
        }
    }
}