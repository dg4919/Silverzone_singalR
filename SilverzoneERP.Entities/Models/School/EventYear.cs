using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class EventYear : AuditableEntity<long>
    {        
        public int Event_Year { get; set; }      

        public  long EventId { set; get; }

        [Required]
        public long EventFee { set; get; }
        [Required]
        public long RetainFee { set; get; }
        //[Required]
        //public int FromClass { set; get; }
        //[Required]
        //public int ToClass { set; get; }

        public virtual IList<EventYearClass> EventYearClass { set; get; }
        #region ForeignKey

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        #endregion
    }
}
