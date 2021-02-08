using System;
using MongoDB.Bson;

namespace MongoDBLib.Models
{
    public class Movie
    {    
        public ObjectId MovieId { get; set; }

        public string Name { get; set; }
        
        public string Director { get; set; }

        public Actor[] Actors { get; set; }
         
        public int Year { get; set; }
        
        public int Age
        {
            get { return DateTime.Now.Year - this.Year; }
        }
 
        public BsonDocument Metadata { get; set; }
    }
}
