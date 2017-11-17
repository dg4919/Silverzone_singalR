using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class Freelance:Entity<long>
    {
        [MaxLength(50)]
        public string YourName { get; set; }

        [MaxLength(50)]
        public string EmailId { get; set; }

        [MaxLength(12)]
        public string Mobile { get; set; }

        [MaxLength(12)]
        public string Phone { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }

        [DataType(DataType.PostalCode)]
        public string PinCode { get; set; }

        public int Age { get; set; }
        public genderType Gender { get; set; }

        [MaxLength(50)]
        public string EduQualification { get; set; }

        [MaxLength(50)]
        public string OthQualification { get; set; }

        [MaxLength(50)]
        public string Profession { get; set; }

        [MaxLength(50)]
        public string HowDid { get; set; }
        
        
    }
}
