using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class ERPuser : UserCommon
    {
        [MaxLength(200)]
        public string UserAddress { set; get; }

        public virtual IEnumerable<UserPermission> UserPermission { set; get; }
    }
}
