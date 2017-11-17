using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class Profile:Entity<long>
    {
        [Required, MaxLength(50)]
        public string ProfileName { get; set; }
    }
}
