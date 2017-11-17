using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class SchoolShippingAddress : Entity<long>
    {
        #region Property
        
        public long SchId { get; set; }

        [MaxLength(200)]
        public string ShipAddress { get; set; }

        #endregion

        #region ForeignKey

        [ForeignKey("SchId")]
        public virtual School School { get; set; }

        #endregion
    }
}
