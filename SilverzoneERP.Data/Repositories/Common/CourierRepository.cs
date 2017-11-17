using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;
namespace SilverzoneERP.Data
{
    public class CourierRepository : BaseRepository<Courier>, ICourierRepository
    {
        public CourierRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public IQueryable<Courier> GetActive()
        {
            return FindBy(x => x.Status == true);  // there will be always 1 record
        }

        public Courier Get(long CouriorId)
        {
            return _dbContext.Couriers.FirstOrDefault(x => x.Id == CouriorId);
        }
        public bool Exists(string CourierName)
        {
            return _dbContext.Couriers.FirstOrDefault(x => x.Courier_Name == CourierName) == null ? false : true;
        }
        public bool Exists(long CouriorId, string CourierName)
        {
            return _dbContext.Couriers.FirstOrDefault(x => x.Id != CouriorId && x.Courier_Name == CourierName) == null ? false : true;
        }
    }
}
