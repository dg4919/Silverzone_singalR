using SilverzoneERP.Entities.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class School : AuditableEntity<long>
    {
        #region Property
           
        public  long SchCode { get; set; }
        
        [Required]
        [MaxLength(50)]        
        public string SchName { get; set; }

        [Required]
        [MaxLength(150)]        
        public string SchAddress { get; set; }
      
        [MaxLength(150)]
        public string SchAltAddress { get; set; }

        public long ZoneId { get; set; }

        public long CountryId { get; set; }

        public long StateId { get; set; }

        public long DistrictId { get; set; }

        public long CityId { get; set; }

        public Nullable<long> SchCategoryId { get; set; }

        public Nullable<long> SchGroupId { get; set; }


        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string SchEmail { get; set; }

       
        [MaxLength(100)]
        public string SchWebSite { get; set; }

       
        public Nullable<long> SchPhoneNo { get; set; }

        
        public Nullable<long> SchFaxNo { get; set; }

        [Required]      
        public long SchPinCode { get; set; }


      
        [MaxLength(50)]
        public string SchBoard { get; set; }

      
        [MaxLength(50)]
        public string SchAffiliationNo { get; set; }

        #endregion

        #region  ForeignKey

        [ForeignKey("ZoneId")]
        public virtual Zone Zone { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        [ForeignKey("SchCategoryId")]
        public virtual SchoolCategory Category { get; set; }

        [ForeignKey("SchGroupId")]
        public virtual SchoolGroup SchoolGroup { get; set; }

        #endregion

        #region IEnumerable

        public virtual IEnumerable<SchoolRemarks> SchoolRemarks { get; set; }
        public virtual IEnumerable<SchoolShippingAddress> ShippingAddress { get; set; }
        public virtual IList<BlackListedSchool> BlackListed { get; set; }
        public virtual IList<EventManagement> EventManagement { get; set; }
        public virtual IList<Contact> Contact { get; set; }

        public virtual IList<SchoolLog> SchoolLog { get; set; }
        #endregion
    }
}
