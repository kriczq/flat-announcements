using Flannounce.Domain.Parser.Utils;
using Flannounce.Model.DAO;

namespace Flannounce.Domain.Parser
{
    public class StreetToCompare : Street
    {
        public string CleanedName { get; set; }

        public StreetToCompare(Street street)
        {
            this.City = street.City;
            this.Name = street.Name;
            this.Id = street.Id;
            this.CleanedName = street.Name.CleanCharacters();;
        }
    }
}