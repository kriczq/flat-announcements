using System.Collections.Generic;
using Flannounce.Configuration;
using Flannounce.Domain.Model;
using MongoDB.Driver;

namespace Flannounce.Controllers
{
    public class FlatService :IFlatService
    {
        private readonly IMongoCollection<Flat> _flats;

        public FlatService(IFlannounceDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _flats = database.GetCollection<Flat>(settings.FlatsCollectionName);
        }

        public List<Flat> Get() =>
            _flats.Find(flat => true).ToList();

        public Flat Get(string id) =>
            _flats.Find<Flat>(student => student.FlatId == id).FirstOrDefault();

        public Flat Create(Flat student) {
            _flats.InsertOne(student);
            return student;
        }

        public void Update(string id, Flat studentIn) =>
            _flats.ReplaceOne(student => student.FlatId == id, studentIn);

        public void Remove(Flat studentIn) =>
            _flats.DeleteOne(student => student.FlatId == studentIn.FlatId);

        public void Remove(string id) => 
            _flats.DeleteOne(student => student.FlatId == id);
    }
}