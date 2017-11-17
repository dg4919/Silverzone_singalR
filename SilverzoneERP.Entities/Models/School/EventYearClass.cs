using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class EventYearClass : AuditableEntity<long>
    {
        public long EventYearId { set; get; }

        [ForeignKey("EventYearId")]
        public virtual EventYear EventYear { get; set; }

        public long ClassId { set; get; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

    }

}
