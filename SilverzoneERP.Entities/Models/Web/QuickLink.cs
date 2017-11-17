using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class QuickLink:Entity<long>
    {
        [MaxLength(50)]
        [Required(ErrorMessage="Title Required")]
        public string Title { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Url Required")]
        public string Url { get; set; }
        
    }
}
