using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    class InventorySourceRepository : BaseRepository<InventorySource>, IinventorySourceRepository
    {
        public InventorySourceRepository(SilverzoneERPContext context) : base(context) { }

        public override IQueryable<InventorySource> GetAll()
        {
            return _dbset.Where(x => x.Status == true);
        }
    }
}
