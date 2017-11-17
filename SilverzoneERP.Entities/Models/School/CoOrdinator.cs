using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class CoOrdinator: AuditableEntity<long>
    {
        #region Property

        public long EventManagementId { get; set; }

        //public long DesgId { get; set; }

        public long TitleId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CoOrdName { get; set; }

      
        public Nullable<long> CoOrdMobile { get; set; }

        public Nullable<long> CoOrdAltMobile1 { get; set; }

        public Nullable<long> CoOrdAltMobile2 { get; set; }

       
        [MaxLength(50)]
        public string CoOrdEmail { get; set; }

        [MaxLength(50)]
        public string CoOrdAltEmail1 { get; set; }

        [MaxLength(50)]
        public string CoOrdAltEmail2 { get; set; }
        

        #endregion

        #region ForeignKey

        [ForeignKey("EventManagementId")]
        public virtual EventManagement EventManagement { get; set; }

        //[ForeignKey("DesgId")]
        //public virtual Designation Designation { get; set; }

        [ForeignKey("TitleId")]
        public virtual Title Title { get; set; }

        #endregion
    }
}
