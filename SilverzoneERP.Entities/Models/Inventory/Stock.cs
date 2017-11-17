using SilverzoneERP.Entities.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class stockInfo_common : AuditableEntity<long>
    {        
        [Required]
        public long BookId { get; set; }
        public virtual Book Book { get; set; }

        public long Quantity { get; set; }
    }

    public class Stock_Master : Entity<long>
    {
        [Required]
        public inventoryType InventoryType { get; set; }

        // if SourceType is dealer > his ID will be strore here
        public long SourceInfo_Id { get; set; }
        [ForeignKey("SourceInfo_Id")]
        public virtual InventorySourceDetail SourceDetail { get; set; }

        public string ChallanNo { get; set; }
        [Required]
        public DateTime ChallanDate { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<Dispatch_Master> Dispatch_Masters { get; set; }

        public string Remarks { get; set; }

        public long? dealerAdresId { get; set; }

        // isVerified use to verify challan which is going to outward
        public bool isVerified { get; set; }
    }

    public class Stock : stockInfo_common
    {
        public long Stock_mId { get; set; }

        [ForeignKey("Stock_mId")]
        public virtual Stock_Master Stock_Master { get; set; }

        public long? PO_Id { get; set; }
        [ForeignKey("PO_Id")]
        public virtual PurchaseOrder_Master PO_Master { get; set; }

        // from dealer when inventory outward > we will provide a refrence no > order rcieved by dealer from mobile/email
        //public string Reference { get; set; }
    }

    public class StockLogs : Entity<long>
    {
        public long Stock_Id { get; set; }

        [ForeignKey("Stock_Id")]
        public virtual Stock Stocks { get; set; }

        public long Qunantity { get; set; }

        public long UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
    }

    public class CounterCustomer : AuditableEntity<long>
    {
        public string name { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public string emailId { get; set; }
        public PaymentModeType PaymentMode { get; set; }

        public long StockId { get; set; }
        public virtual Stock_Master StockInfo { get; set; }
        

    }

}
