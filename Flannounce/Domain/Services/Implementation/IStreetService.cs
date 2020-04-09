using System.Collections.Generic;
using Flannounce.Model.DAO;

namespace Flannounce.Domain.Services.Implementation
{
    public interface IStreetService
    {
        List<Street> Get();
    }
}