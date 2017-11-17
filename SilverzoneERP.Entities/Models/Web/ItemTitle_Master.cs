using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class ItemTitle_Master : AuditableEntity<long> 
    {
        public long SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Event Subject { get; set; }

        public long ClassId { get; set; }
        public virtual Class Class { get; set; }

        public long CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual BookCategory BookCategory { get; set; }
    }
}
