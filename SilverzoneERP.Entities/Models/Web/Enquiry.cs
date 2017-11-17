using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class Enquiry:Entity<long>
    {
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string EmailId { get; set; }

        [MaxLength(12)]
        public string Mobile { get; set; }

        [MaxLength(30)]
        public string Subject { get; set; }

        [MaxLength(120)]
        public string Description { get; set; }
        public DateTime QueryDate { get; set; }
    }
}
