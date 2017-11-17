using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class Designation : AuditableEntity<long>
    {
        [Required]
        [MaxLength(50)]
        public string DesgName { set; get; }

        public string DesgDescription { set; get; }
    }
}
