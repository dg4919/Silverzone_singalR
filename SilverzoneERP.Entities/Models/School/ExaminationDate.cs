using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{

    public class ExaminationDate : AuditableEntity<long>
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExamDate { set; get; }
    }
}
