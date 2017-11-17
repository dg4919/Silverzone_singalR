using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class ItemTitle_MasterRepository : BaseRepository<ItemTitle_Master>, IitemTitle_MasterRepository
    {
        public ItemTitle_MasterRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

    }
}
