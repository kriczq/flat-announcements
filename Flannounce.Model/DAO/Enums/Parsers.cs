using System;

namespace Flannounce.Model.DAO.Enums
{
    public static class Parsers
    {
        public static BuildingType Parse(string rawBuildingType)
        {
            var buildingType = BuildingType.Others;

            Enum.TryParse(rawBuildingType, out buildingType);

            return buildingType;
        }
    }
}