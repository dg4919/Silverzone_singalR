using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class Olympiads:Entity<long>
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "Title Required")]
        public string Title { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Description Required")]
        public string Description { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Image Required")]
        public string ImageName { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Link Required")]
        public string Link { get; set; }
        
    }
}
