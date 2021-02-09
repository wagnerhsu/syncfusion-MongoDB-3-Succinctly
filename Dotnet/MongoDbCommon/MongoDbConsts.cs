namespace MongoDbCommon
{
    public class MongoDbConsts
    {
        public const string LocalMongoDbConnectionString = "mongodb://localhost:27017";

        public static class MoviesDb
        {
            public const string DatabaseName = "moviesDb";
            public const string MoviesBsonCollection = "movies_bson";
        }
    }
}