using SilverzoneERP.Entities.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class PurchaseOrder_Master : Entity<long>
    {
        public long PO_Number { get; set; }
        [Required]
        public DateTime PO_Date { get; set; }

        public orderSourceType From { get; set; }
        public long srcFrom { get; set; }

        public orderSourceType To { get; set; }
        public long srcTo { get; set; }

        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        // like if order come from dealer & given through SMS
        public string Remarks { get; set; }

        // for school its must be verified to PO
        public bool isVerified { get; set; }
    }

    public class PurchaseOrder : stockInfo_common
    {
        public long PO_mId { get; set; }
        [ForeignKey("PO_mId")]
        public virtual PurchaseOrder_Master PO_Master { get; set; }

        public double Rate { get; set; }

        // when order will be adjust > like 1000 item order = 995 order rcieved => 5 item can be adjusted
        public bool is_Adjusted { get; set; }
        public string Adjust_Remarks { get; set; }
    }

    public class PurchaseOrderLogs : Entity<long>
    {
        public long PO_Id { get; set; }

        [ForeignKey("PO_Id")]
        public virtual PurchaseOrder PurchaseOrders { get; set; }

        public long Qunantity { get; set; }

        public long UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
    }

}
