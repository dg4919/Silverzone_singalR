using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class BookContent:Entity<long>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }
        

        public long BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
