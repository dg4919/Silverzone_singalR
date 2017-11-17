using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    class InventorySourceDetailRepository : BaseRepository<InventorySourceDetail>, IinventorySourceDetailRepository
    {
        public InventorySourceDetailRepository(SilverzoneERPContext context) : base(context) { }

        public bool isRecordExist(string name, string pan, string tan, long sourceId)
        {
            return _dbset.Any(x => (x.SourceName.Equals(name)
                                || x.SourcePAN.Equals(pan)
                                || x.SourceTAN.Equals(tan))
                                && x.SourceId == sourceId);
        }

        public bool isRecordExist(long id, string name, string pan, string tan, long sourceId)
        {
            return _dbset.Any(x => (x.SourceName.Equals(name)
                                || x.SourcePAN.Equals(pan)
                                || x.SourceTAN.Equals(tan))
                                && x.Id != id
                                && x.SourceId == sourceId);
        }

        public IQueryable<InventorySourceDetail> FilerBySourceId(long srcId)
        {
            return FindBy(x => x.SourceId == srcId);
        }
     
    }

    class DealerBookDiscountRepository : BaseRepository<DealerBookDiscount>, IDealerBookDiscountRepository
    {
        public DealerBookDiscountRepository(SilverzoneERPContext context) : base(context) { }
    }

    class DealerSceondaryAddressRepository : BaseRepository<DealerSecondaryAddress>, IDealerSceondaryAddressRepository
    {
        public DealerSceondaryAddressRepository(SilverzoneERPContext context) : base(context) { }
    }
}
