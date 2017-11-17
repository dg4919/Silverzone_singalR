using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class CityRepository:BaseRepository<City>,ICityRepository
    {
        public CityRepository(SilverzoneERPContext context) : base(context) { }
        public bool Exists(string CityName, long CountryId, long ZoneId, long StateId, long DistrictId)
        {
            return _dbset.FirstOrDefault(x=>x.CityName.Trim().ToLower()== CityName.Trim().ToLower()&&x.CountryId==CountryId&&x.ZoneId==ZoneId&&x.StateId==StateId&&x.DistrictId==DistrictId) == null ? false : true;
        }

        public bool Exists(long CityId, string CityName, long CountryId, long ZoneId, long StateId, long DistrictId)
        {
            return _dbset.FirstOrDefault(x =>x.Id!=CityId && x.CityName.Trim().ToLower() == CityName.Trim().ToLower() && x.CountryId == CountryId && x.ZoneId == ZoneId && x.StateId == StateId && x.DistrictId == DistrictId) == null ? false : true;
        }
        public City Get(long CityId)
        {
            return _dbset.FirstOrDefault(x=>x.Id==CityId);
        }
    }
}
