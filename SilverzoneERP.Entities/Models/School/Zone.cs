using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class Zone:AuditableEntity<long>
    {
        #region Property

        [Required]
        [MaxLength(100)]        
        public string ZoneName { get; set; }

        public long CountryId { get; set; }
        #endregion

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        #region IEnumerable
        public virtual IList<State> States { get; set; }

        //public virtual IList<Country> Countries { get; set; }
        //public virtual IList<State> States { get; set; }
        //public virtual IList<District> Districts { get; set; }
        //public virtual IList<City> Cities { get; set; }
        //public virtual IList<School> Schools { get; set; }

        #endregion
    }
}
