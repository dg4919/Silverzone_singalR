using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;

namespace SilverzoneERP.Entities.Models
{
    public class BookBundle : AuditableEntity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal bundle_totalPrice { get; set; }
        public decimal books_totalPrice { get; set; }

        public long ClassId { get; set; }
        public virtual Class Class { get; set; }

        public virtual ICollection<BookBundleDetails> bundle_details { get; set; }
    }
}
