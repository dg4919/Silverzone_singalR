using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace SilverzoneERP.Entities.Models
{
    public class CoOrdinatingTeacher : AuditableEntity<long>
    {
        #region Property
    
        public long EventManagementId { get; set; }
        [Required]
        public long TitleId { set; get; }
        [Required]
        [MaxLength(50)]
        public string Name { set; get; }
       
        public Nullable<long> MobileNo { set; get;}
        public Nullable<long> AltMobileNo1 { set; get; }
        public Nullable<long> AltMobileNo2 { set; get; }

        
        [MaxLength(50)]
        public string EmailID { set; get; }

        [MaxLength(50)]
        public string AltEmailID1 { set; get; }

        [MaxLength(50)]
        public string AltEmailID2 { set; get; }

        public Nullable<int> No_Of_Selected_Ques { set; get; }       
        #endregion

        #region ForeignKey

        [ForeignKey("EventManagementId")]
        public virtual EventManagement EventManagement { get; set; }
        [ForeignKey("TitleId")]
        public virtual Title Title { get; set; }

        #endregion
    }
}
