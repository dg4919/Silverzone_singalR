using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class Contact: Entity<long>
    {
        #region Property

        public long SchId { set; get; }
        public long DesgId { set; get; }
        public long TitleId { set; get; }
        public string ContactName { set; get; }
        public short ContactType { set; get; }
        public bool AddressTo { set; get; }

        
        public Nullable<long> ContactMobile { set; get; }

        public Nullable<long> ContactAltMobile1 { set; get; }

        public Nullable<long> ContactAltMobile2 { set; get; }
        
        [MaxLength(50)]
        public string ContactEmail { set; get; }

        [MaxLength(50)]
        public string ContactAltEmail1 { set; get; }

        [MaxLength(50)]
        public string ContactAltEmail2 { set; get; }

        public long ContactYear { set; get; }
        #endregion

        #region ForeignKey

        [ForeignKey("SchId")]
        public virtual School School { get; set; }

        [ForeignKey("DesgId")]
        public virtual Designation Designation { get; set; }

        [ForeignKey("TitleId")]
        public virtual Title Title { get; set; }

        #endregion
    }
}
