using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IZoneRepository:IRepository<Zone>
    {
        bool Exists(long CountryId, string ZoneName);
        bool Exists(long CountryId, long ZoneId, string ZoneName);
        Zone Get(long ZoneId);
    }
}
