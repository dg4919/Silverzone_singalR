using System.Linq;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class ZoneRepository : BaseRepository<Zone>, IZoneRepository
    {
        public ZoneRepository(SilverzoneERPContext context) : base(context) { }

        public bool Exists(long CountryId, string ZoneName)
        {
            return _dbset.FirstOrDefault(x =>x.CountryId==CountryId && x.ZoneName.Trim().ToLower() == ZoneName.Trim().ToLower()) == null ? false : true;
        }
        public bool Exists(long CountryId, long ZoneId, string ZoneName)
        {
            return _dbset.FirstOrDefault(x => x.CountryId == CountryId && x.Id != ZoneId && x.ZoneName.Trim().ToLower() == ZoneName.Trim().ToLower()) == null ? false : true;
        }
        public Zone Get(long ZoneId)
        {
            return _dbset.FirstOrDefault(x => x.Id == ZoneId);
        }
    }
}
