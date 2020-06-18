using Flannounce.Configuration;
using Flannounce.Model.DAO;
using MongoDB.Driver;

namespace Flannounce.Domain.DB
{
    public class DbClient : IDbClient
    {
        public IMongoCollection<Announce> Announces { get; set; }
        public IMongoCollection<Announce> CleanedAnnounces { get; set; }
        public IMongoCollection<Street> Streets { get; set; }

        public DbClient(IFlannounceDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Announces = database.GetCollection<Announce>(settings.AnnouncesCollectionName);
            CleanedAnnounces = database.GetCollection<Announce>(settings.CleanedAnnouncesCollectionName);
            Streets = database.GetCollection<Street>(settings.StreetsCollectionName);
        }
    }
}