using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SilverzoneERP.Entities.Models
{
    public class City:AuditableEntity<long>
    {
        #region Property

        [Required]
        [MaxLength(100)]
        public string CityName { get; set; }

        public long CountryId { get; set; }

        public long ZoneId { get; set; }        

        public long StateId { get; set; }

        public long DistrictId { get; set; }
        
        #endregion

        #region ForeignKey

        [ForeignKey("ZoneId")]
        public virtual Zone Zone { get; set; }
        
        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        #endregion

        #region IEnumerable

       // public virtual IEnumerable<School> Schools { get; set; }

        #endregion
    }
}
