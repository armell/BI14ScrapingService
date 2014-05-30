using BI2014.Scrapping.Properties;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Mongo
{
    public class MongoService<T>
    {
        private MongoClient _client;
        private MongoServer _server;
        private MongoDatabase _database;
        public MongoService()
        {
            _client = new MongoClient(Settings.Default.MongoDB);
            _server = _client.GetServer();
            _database = _server.GetDatabase("MongoBI2014");
        }

        public MongoDatabase Database { get { return _database; } }

        private  MongoCollection<T> GetCollection(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public void SaveCollection(string collectionName, ICollection<T> collection)
        {
            var mongoCollection = GetCollection(collectionName);

            mongoCollection.InsertBatch(collection);
        }

        public IEnumerable<T> GetAll(string collectionName)
        {
            return GetCollection(collectionName).FindAll();
        }

        public void RemoveAll(string collectionName)
        {
            GetCollection(collectionName).RemoveAll();
        }

        public void UpdateAllCourseMember()
        {

            
        }
    }
}
