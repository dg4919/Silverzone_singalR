using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface IinventorySourceDetailRepository : IRepository<InventorySourceDetail>
    {
        bool isRecordExist(string name, string pan, string tan, long sourceId);
        bool isRecordExist(long id, string name, string pan, string tan, long sourceId);
        IQueryable<InventorySourceDetail> FilerBySourceId(long srcId);
    }

    public interface IDealerBookDiscountRepository : IRepository<DealerBookDiscount>
    {
    }

    public interface IDealerSceondaryAddressRepository : IRepository<DealerSecondaryAddress>
    {
    }

}
