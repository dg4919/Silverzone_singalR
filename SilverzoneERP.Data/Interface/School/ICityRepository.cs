using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface ICityRepository : IRepository<City>
    {
        bool Exists(string CityName, long CountryId, long ZoneId, long StateId, long DistrictId);
        bool Exists(long CityId, string CityName, long CountryId, long ZoneId, long StateId, long DistrictId);
        City Get(long CityId);
    }
}
