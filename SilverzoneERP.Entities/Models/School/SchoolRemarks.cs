using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class SchoolRemarks : AuditableEntity<long>
    {
        #region
        
        public long SchId { get; set; }

        [MaxLength(200)]
        public string SchRemarks { get; set; }

        #endregion

        #region ForeignKey

        [ForeignKey("SchId")]
        public virtual School School { get; set; }

        #endregion


    }
}
