using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;

namespace SilverzoneERP.Entities.Models
{
    public class InventorySource : AuditableEntity<long>
    {
        public string SourceName { get; set; }
        public string SourceDesciprtion { get; set; }

        public virtual ICollection<InventorySourceDetail> SourceDetails { get; set; }
        //public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        //public virtual ICollection<PurchaseOrder_Master> PurchaseOrder_Masters { get; set; }
    }
}
