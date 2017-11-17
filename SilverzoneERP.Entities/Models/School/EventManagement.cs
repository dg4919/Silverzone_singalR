using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class EventManagement : AuditableEntity<long>
    {
        #region Property
        [Required]
        public long SchId { get; set; }

       
        public long RegNo { get; set; }

        [Required]
        public long EventId { get; set; }

        public long EventManagementYear { set; get; } = DateTime.Now.Year;
        
        public Nullable<int> PostalCommunication { set; get; }

        public Nullable<long> CourierId { set; get; }

        public string OtherCourier { set; get; }

        public string EnrollmentOrderSummary { set; get; }

        public long TotalEnrollmentSummary { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ExamDate { set; get; }

        public Nullable<bool> IsParticipate { set; get; }

        #endregion

        #region ForeignKey

        [ForeignKey("SchId")]
        public virtual School School { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [ForeignKey("CourierId")]
        public virtual Courier Courier { get; set; }
       

        public virtual IList<CoOrdinator> CoOrdinator{ get; set; }
        public virtual IList<CoOrdinatingTeacher> CoOrdinatingTeacher { get; set; }
        public virtual IList<EnrollmentOrder> EnrollmentOrder { get; set; }

        public virtual IList<FeePayment> FeePayment { get; set; }
        public virtual IList<StudentEntry> StudentEntry { get; set; }
        #endregion

    }
}
