using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SilverzoneERP.Entities.Models
{
    public class District : AuditableEntity<long>
    {
        #region Property

        [Required]
        [MaxLength(100)]
        public string DistrictName { get; set; }

        public long CountryId { get; set; }

        public long ZoneId { get; set; }

        public long StateId { get; set; }

        #endregion

        #region ForeignKey

        [ForeignKey("ZoneId")]
        public virtual Zone Zone { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        #endregion

        #region

        public virtual IList<City> Cities { get; set; }
        //public virtual IList<School> Schools { get; set; }

        #endregion
    }
}
