using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;
namespace SilverzoneERP.Entities.Models
{
    public class StudentAttendance : AuditableEntity<long>
    {
        #region Property
               
        public string AnswerJSON { set; get; }

        public int Type { set; get;}
      
        [ForeignKey("Id")]
        public virtual StudentEntry StudentEntry { get; set; }

        #endregion      
    }
}
