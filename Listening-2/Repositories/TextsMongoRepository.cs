using listening.Models.Text;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Repositories
{
    public class TextsMongoRepository : IRepository<Text, string>
    {
        private const string TextId = "TextId";

        private string _collectionName;
        private IMongoClient _client;
        private IMongoDatabase _dataBase;
        private IMongoCollection<Text> _collection;

        public TextsMongoRepository(IConfigurationRoot configuration)
        //string url = "mongodb://localhost:27017",
        //string dbName = "EnglishListeningDb",
        //string collectionName = "Texts")
        {
            _client = new MongoClient(configuration["MongoDB:Url"]);
            _dataBase = _client.GetDatabase(configuration["MongoDB:DataBaseName"]);
            _collectionName = configuration["MongoDB:CollectionName"];
            _collection = _dataBase.GetCollection<Text>(_collectionName);
        }

        public void Delete(string id)
        {
            var filter = Builders<Text>.Filter.Eq(TextId, ObjectId.Parse(id));
            _collection.DeleteOne(filter);
        }

        public IQueryable<Text> GetAll()
        {
            return _collection.Find(new BsonDocument()).ToList().AsQueryable();
        }

        public Text GetById(string id)
        {
            //var collection = _dataBase.GetCollection<Text>(_collectionName);
            var filter = Builders<Text>.Filter.Eq(TextId, ObjectId.Parse(id));
            return _collection.Find(filter).ToList().First();
        }

        public void Insert(Text item)
        {
            //var collection = _dataBase.GetCollection<Text>(_collectionName);
            _collection.InsertOne(item);
        }

        public void Update(Text item)
        {
            //var collection = _dataBase.GetCollection<Text>(_collectionName);
            var filter = Builders<Text>.Filter.Eq(TextId, item.TextId);
            var update = Builders<Text>.Update
                            .Set(nameof(item.Title), item.Title)
                            .Set(nameof(item.SubTitle), item.SubTitle)
                            .Set(nameof(item.WordsInParagraphs), item.WordsInParagraphs)
                            .Set(nameof(item.AudioName), item.AudioName)
                            .Set(nameof(item.SymbolsCount), item.SymbolsCount);
            _collection.UpdateOne(filter, update);
        }
    }
}
