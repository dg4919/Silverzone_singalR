using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class CourierModeRepository : BaseRepository<CourierMode>, ICourierModeRepository
    {
        public CourierModeRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public IQueryable<CourierMode> GetByCourierId(int CourierId)
        {
            return FindBy(x => x.CourierId == CourierId && x.Status == true);  // there will be always 1 record
        }
    }
}
