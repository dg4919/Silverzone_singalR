using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class OrderStatusReason : Entity<long>
    {
        public string Reason { get; set; }
    }
}
