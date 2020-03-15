using System.Runtime.Serialization;

namespace Flannounce.Model.DAO.Enums
{
    public enum BuildingType
    {
        [EnumMember(Value = "Blok")]
        Blok,
        Kamienica,
        Apartamentowiec,
        Loft,
        Others
    }
}