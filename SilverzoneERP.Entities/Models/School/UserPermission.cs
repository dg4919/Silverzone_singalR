using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class UserPermission : Permission
    {
        #region Property
        public long UserId { set; get; }
      
        public long FormId { set; get; }
      
      

        #endregion

        #region ForeignKey

        [ForeignKey("FormId")]
        public virtual FormManagement FormManagement { set; get; }

      
        [ForeignKey("UserId")]
        public virtual User User { set; get; }

        #endregion
    }
}
