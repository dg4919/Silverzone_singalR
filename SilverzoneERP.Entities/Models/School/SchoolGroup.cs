using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class SchoolGroup : AuditableEntity<long>
    {        
        [Required]
        [MaxLength(100)]        
        public string SchoolGroupName { get; set; }        
    }
}
