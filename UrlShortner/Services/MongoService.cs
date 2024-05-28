using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UrlShortner.Models;
namespace UrlShortner.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<shortenerModel> _service;
        private readonly IMongoCollection<VisitorCount> _visitorservice;

        public MongoService()
        {
            //var connectionString = "mongodb://localhost:27017/";
            //var databaseName = "urlshortener";
            //var collectionName = "shortenerCollection";

            var connectionString = "mongodb+srv://arbazaliMongo:PfKKEKtYYNHQYovy@urlshortnercluster.tr3wymk.mongodb.net/";
            var databaseName = "UrlShortnerDb";
            var collectionName = "shortenerCollection";
            var collectionName2 = "visitorsCollection";


            //Console.WriteLine(connectionString);
            //Console.WriteLine(databaseName);
            //Console.ReadLine();
            var mongoClientResults = new MongoClient(connectionString);
            var mongoDatabaseResults = mongoClientResults.GetDatabase(databaseName);
            _service = mongoDatabaseResults.GetCollection<shortenerModel>(collectionName);
            _visitorservice = mongoDatabaseResults.GetCollection<VisitorCount>(collectionName2);
        }

        public  void InsertDatatoMongo(shortenerModel model)
        {
            _service.InsertOne(model);
        }

        public shortenerModel FindByShortCode(string shortCode)
        {
            return _service.Find(s => s.ShortenedUrl == shortCode).FirstOrDefault();
        }

        public void insertifnone(VisitorCount visitor)
        {
            _visitorservice.InsertOne(visitor);
        }

        public VisitorCount GetCountcollection()
        {
            return _visitorservice.Find(_ => true).FirstOrDefault();
        }

        public void IncrementVisitorCount()
        {
            var visitorCount = _visitorservice.Find(_ => true).FirstOrDefault();
            if (visitorCount == null)
            {
                visitorCount = new VisitorCount { Count = 1 };
                _visitorservice.InsertOne(visitorCount);
            }
            else
            {
                var update = Builders<VisitorCount>.Update.Inc(v => v.Count, 1);
                _visitorservice.UpdateOne(v => v.Id == visitorCount.Id, update);
            }
        }
    }
}
