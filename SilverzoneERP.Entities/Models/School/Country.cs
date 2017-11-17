using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class Country:AuditableEntity<long>
    {

        #region Property

        [Required]
        [MaxLength(100)]
        public string CountryName { get; set; }

        //public long ZoneId { get; set; }

        #endregion

        #region ForeignKey

        //[ForeignKey("ZoneId")]
        //public virtual Zone Zone { get; set; }

        #endregion

        #region IEumerable
        public virtual IList<Zone> Zones { get; set; }
        //public virtual IList<State> States { get; set; }        
        //public virtual IList<District> Districts { get; set; }
        //public virtual IList<City> Cities { get; set; }
        //public virtual IList<School> Schools { get; set; }

        #endregion
    }
}
