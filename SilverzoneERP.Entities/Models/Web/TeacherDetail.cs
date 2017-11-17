using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class TeacherDetail : Entity<long>
    {
        [ForeignKey("Id")]
        public User User { get; set; }

        [Required]
        public string SchoolName { get; set; }

        public string SchoolCode { get; set; }

        [MaxLength(200)]
        public string SchoolAddress { get; set; }

        [Required]
        public int PinCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }
       
        public long ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
