using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public abstract class UserCommon : AuditableEntity<long>
    {
        #region Property

        [MaxLength(100)]
        public string UserName { get; set; }

        [MaxLength(200)]
        public string Password { get; set; }

        [MaxLength(200)]
        public string ProfilePic { get; set; }

        // type is suffix if create colum in DB to identify column is a type of Enum
        public genderType? GenderType { get; set; }       // get from Enum

        [Column(TypeName = "Date")]         // to create a date type column in DB
        public DateTime? DOB { get; set; }          // 2 ways making value nullable

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Enter Valid Email ID")]
        public string EmailID { get; set; }

        public string MobileNumber { get; set; }

        public string IPAddress { get; set; }
        public string Browser { get; set; }
        public string OperatingSystem { get; set; }
        public string Location { get; set; }

        public long RoleId { get; set; }
        public virtual Role Role { get; set; }

        #endregion
    }
}
