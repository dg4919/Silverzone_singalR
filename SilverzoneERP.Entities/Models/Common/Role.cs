using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class Role : AuditableEntity<long>
    {
        #region Property

        [MaxLength(100)]
        public string RoleName { get; set; }

        [MaxLength(200)]
        public string RoleDescription { get; set; }

        #endregion

        #region IEnumerable

        public virtual IEnumerable<RolePermission> RolePermission { set; get; }

        #endregion
    }
}
