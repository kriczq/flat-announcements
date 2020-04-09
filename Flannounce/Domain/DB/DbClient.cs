using Flannounce.Configuration;
using Flannounce.Model.DAO;
using MongoDB.Driver;

namespace Flannounce.Domain.DB
{
    public class DbClient : IDbClient
    {
        public IMongoCollection<Announce> Announces { get; set; }
        public IMongoCollection<Street> Streets { get; set; }

        public DbClient(IFlannounceDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Announces = database.GetCollection<Announce>(settings.AnnouncesCollectionName);
            Streets = database.GetCollection<Street>(settings.StreetsCollectionName);
        }
    }
}