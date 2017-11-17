using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class BookBundleDetails : Entity<long>
    {
        public long BundleId { get; set; }

        [ForeignKey("BundleId")]
        public virtual BookBundle bookBundle { get; set; }

        public long BookId { get; set; }

        [ForeignKey("BookId")]
        public virtual Book book { get; set; }
    }
}
