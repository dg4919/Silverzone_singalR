using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class InventorySourceDetail : AuditableEntity<long>
    {
        public long SourceId { get; set; }

        public string SourceName { get; set; }
        public string SourceAddress { get; set; }
        public string Contact_Person_Name { get; set; }

        public string SourceMobile { get; set; }
        public string SourceLandline { get; set; }
        public string SourceEmail { get; set; }
        public string SourcePAN { get; set; }
        public string SourceTAN { get; set; }

        [ForeignKey("SourceId")]
        public virtual InventorySource Source { get; set; }

        public long? City_Id { get; set; }
        public int PinCode { get; set; }

        [ForeignKey("City_Id")]
        public virtual City City { get; set; }

        public virtual ICollection<DealerSecondaryAddress> DealerSceondaryAddressess { get; set; }
        public virtual ICollection<DealerBookDiscount> DealerBookDiscounts { get; set; }
    }

    public class DealerSecondaryAddress : Entity<long>
    {
        public string Address { get; set; }

        public bool isDefault { get; set; }

        public long SourceDetailId { get; set; }
        [ForeignKey("SourceDetailId")]
        public virtual InventorySourceDetail InventorySourceDetails { get; set; }
    }

    public class DealerBookDiscount : AuditableEntity<long>
    {
        public long SourceDetailId { get; set; }
        [ForeignKey("SourceDetailId")]
        public virtual InventorySourceDetail InventorySourceDetails { get; set; }

        public long BookCategoryId { get; set; }
        [ForeignKey("BookCategoryId")]
        public BookCategory BookCategorys { get; set; }

        public decimal DiscountPercentage { get; set; }
    }

}
