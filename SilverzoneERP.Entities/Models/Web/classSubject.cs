using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class classSubject : AuditableEntity<long> 
    {
        public long ClassId { get; set; }
        public virtual Class Class { get; set; }

        public long SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Event Subjects { get; set; }

    }
}
