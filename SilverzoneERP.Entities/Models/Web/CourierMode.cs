using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class CourierMode : AuditableEntity<long>
    {
        [Required]
        public string Courier_Mode { get; set; }

        [Required]
        public long CourierId { get; set; }
        public virtual Courier Courier { get; set; }
    }
}
