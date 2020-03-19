using System.Collections.Generic;
using Flannounce.Configuration;
using Flannounce.Model.DAO;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Flannounce.Controllers
{
    public class AnnounceService : IAnnounceService
    {
        private readonly IMongoCollection<Announce> _announces;

        public AnnounceService(IFlannounceDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _announces = database.GetCollection<Announce>(settings.AnnouncesCollectionName);
        }

        public List<Announce> Get() =>
            _announces.Find(flat => true).ToList();

        public Announce Get(string id) =>
            _announces.Find<Announce>(a => a.Id == id).FirstOrDefault();

        public Announce Create([FromBody] Announce announce) {
            _announces.InsertOne(announce);
            return announce;
        }

        public void Update(string id, Announce announceIn) =>
            _announces.ReplaceOne(a => a.Id == id, announceIn);

        public void Remove(Announce announceIn) =>
            _announces.DeleteOne(a => a.Id == announceIn.Id);

        public void Remove(string id) => 
            _announces.DeleteOne(a =>a.Id == id);
    }
}