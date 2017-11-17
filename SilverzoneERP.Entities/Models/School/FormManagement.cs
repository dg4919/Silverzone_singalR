using SilverzoneERP.Entities.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class FormManagement : Entity<long>
    {
        #region Property

        public string FormName { set; get; }
        public string FormUrl { set; get; }
        public int FormLevel { set; get; }
        public int FormOrder { set; get; }

        public Nullable<long> FormParentId { set; get; }

        [ForeignKey("FormParentId")]
        public virtual IList<FormManagement> ChildFormManagement { set; get; }

        #endregion
    }
}
