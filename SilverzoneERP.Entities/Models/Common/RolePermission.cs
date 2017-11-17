using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class RolePermission: Permission
    {
        #region Property

        public long RoleId { get; set; }

      

        public long FormId { get; set; }

        #endregion

        #region ForeignKey

        [ForeignKey("RoleId")]
        public virtual Role Role { set; get; }

        [ForeignKey("FormId")]
        public virtual FormManagement FormManagement { set; get; }

        #endregion
    }
}
  