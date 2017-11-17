using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class OrderDetail:Entity<long>
    {
        public long OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int Quantity { get; set; }

        public long? BookId { get; set; }
        public virtual Book Book { get; set; }      // Book + Id = BookId is FK 

        public long? BundleId { get; set; }
        public virtual BookBundle Bundle { get; set; }

        public decimal UnitPrice { get; set; }

        // book is a type of bundle or book
        public bookType bookType { get;  set; }
    }
}
