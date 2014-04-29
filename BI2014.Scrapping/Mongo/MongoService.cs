using MongoDB.Driver;
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
            _client = new MongoClient(@"mongodb://MongoBI2014:pUcCzGZVbdKhMagxtRPljOYUp_2A1F81XhuxwVIbhn4-@ds027748.mongolab.com:27748/MongoBI2014");
            _server = _client.GetServer();
            _database = _server.GetDatabase("MongoBI2014");
        }

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
    }
}
