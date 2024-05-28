using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace UrlShortner.Models
{
    public class shortenerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public string CreatedTime { get; set; }
    }

    public class VisitorCount
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int Count { get; set; }
       // public string CreatedTime { get; set; }
    }
}
