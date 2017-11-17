using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class InFavourOf : AuditableEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string Name { set; get; }

        public virtual IList<DepositOnBank> DipositOnBank { get; set; }
    }
}
