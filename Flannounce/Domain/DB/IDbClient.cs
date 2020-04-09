using Flannounce.Model.DAO;
using MongoDB.Driver;

namespace Flannounce.Domain.DB
{
    public interface IDbClient
    {
        IMongoCollection<Announce> Announces { get; set; }
        
        IMongoCollection<Street> Streets { get; set; }
    }
}