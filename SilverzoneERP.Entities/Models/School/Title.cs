using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class Title : AuditableEntity<long>
    {
        [Required]
        [MaxLength(50)]
        public string TitleName { set; get; }       
    }
}
