using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class DrownOnBank : AuditableEntity<long>
    {
        [Required]
        [MaxLength(50)]
        public string BankName { set; get; }        
    }
}
