using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class MedalImage:Entity<long>
    {
        [MaxLength(50)]
        public string ImageName { get; set; }
        
    }
}
