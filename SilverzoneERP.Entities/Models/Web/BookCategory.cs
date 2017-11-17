using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class BookCategory:AuditableEntity<long>
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }

        public long? CouponId { get; set; }

        [ForeignKey("CouponId")]             // virtual is must to get > all related (primary & foreign key based) data
        public virtual Coupon Coupons { get; set; }
    }
}
