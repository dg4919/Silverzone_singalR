using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SilverzoneERP.Entities.Models
{
    public class State:AuditableEntity<long>
    {
        #region Property

        [Required]
        [MaxLength(100)]        
        public string StateName { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 2)]
        public string StateCode { get; set; }

        public long ZoneId { get; set; }

        public long CountryId { get; set; }

        #endregion

        #region ForeignKey

        [ForeignKey("ZoneId")]
        public virtual Zone Zone { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        #endregion

        #region IEnumerable

        public virtual IList<District> Districts { get; set; }
        //public virtual IList<City> Cities { get; set; }
        //public virtual IList<School> Schools { get; set; }

        #endregion
    }
}
