using System.Collections.Generic;
using Flannounce.Domain.DB;
using Flannounce.Model.DAO;
using MongoDB.Driver;

namespace Flannounce.Domain.Services.Implementation
{
    public class StreetService : IStreetService
    {
        private readonly IMongoCollection<Street> _streets;

        public StreetService(IDbClient client)
        {
            _streets = client.Streets;
        }

        public List<Street> Get()
        {
            return _streets.Find(_ => true).ToList();
        }
    }
}