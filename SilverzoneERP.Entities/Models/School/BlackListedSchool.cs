using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class BlackListedSchool : AuditableEntity<long>
    {
        #region Property

        public long SchId { get; set; }
        public bool IsBlocked { get; set; }

        [MaxLength(200)]
        public string BlackListedRemarks { get; set; }

        //public long BlackListedBy { get; set; }
        //public DateTime BlackListedOn { get; set; }
        //public long UnBlockedBy { get; set; }
        //public DateTime UnBlockedOn { get; set; }
        

        #endregion

        #region ForeignKey

        [ForeignKey("SchId")]
        public virtual School School { get; set; }

        #endregion
    }
}
