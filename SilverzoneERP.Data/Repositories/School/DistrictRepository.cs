using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class DistrictRepository : BaseRepository<District>, IDistrictRepository
    {
        public DistrictRepository(SilverzoneERPContext context) : base(context) { }

        public bool Exists(string DistrictName, long CountryId, long ZoneId, long StateId)
        {
            return _dbset.FirstOrDefault(x => x.DistrictName.Trim().ToLower() == DistrictName.Trim().ToLower() && x.CountryId == CountryId && x.ZoneId == ZoneId && x.StateId == StateId) == null ? false : true;
        }
        public bool Exists(long DistrictId, string DistrictName, long CountryId, long ZoneId, long StateId)
        {
            return _dbset.FirstOrDefault(x =>x.Id!=DistrictId && x.DistrictName.Trim().ToLower() == DistrictName.Trim().ToLower() && x.CountryId == CountryId && x.ZoneId == ZoneId && x.StateId == StateId) == null ? false : true;
        }
        public District Get(long DistrictId)
        {
            return _dbset.FirstOrDefault(x=>x.Id==DistrictId);
        }
    }
}
