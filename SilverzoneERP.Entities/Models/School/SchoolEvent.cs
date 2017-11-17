using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public  class SchoolEvent:Entity<long>
    {
        [Required]
        public string SchoolId { get; set; }
        public School School { get; set; }

        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public string EventYear { get; set; }
    }
}
