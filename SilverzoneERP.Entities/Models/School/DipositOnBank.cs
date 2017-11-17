using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class DepositOnBank : AuditableEntity<long>
    {
        [Required]
        [MaxLength(50)]
        public string BankName { set; get; }

        [Required]        
        public long AccountNo { set; get; }

        [Required]
        public long FavourOfId { set; get; }

        [ForeignKey("FavourOfId")]
        public virtual InFavourOf InFavourOf { get; set; }
    }
}
