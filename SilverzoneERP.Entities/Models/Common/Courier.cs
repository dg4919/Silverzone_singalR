using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class Courier : AuditableEntity<long>
    {
        [Required]
        [MaxLength(50)]
        public string Courier_Name { get; set; }
        
        [MaxLength(100)]
        public string Courier_Link { get; set; }

        public virtual IEnumerable<CourierMode> CourierMode { get; set; }
    }
}
