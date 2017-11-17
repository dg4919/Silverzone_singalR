using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class OrderStatusReasonRepository : BaseRepository<OrderStatusReason>, IOrderStatusReasonRepository
    {
        public OrderStatusReasonRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }
}
