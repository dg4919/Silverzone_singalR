using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class BookReview:AuditableEntity<long>
    {
        [MaxLength(300)]
        public string Review { get; set; }
        public string Rating { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
     
    }
}
