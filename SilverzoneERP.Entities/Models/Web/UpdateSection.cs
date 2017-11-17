using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class UpdateSection:Entity<long>
    {
        [MaxLength(20)]
        public string Heading { get; set; }

        [MaxLength(150)]
        public string UpdateLine { get; set; }

        [MaxLength(30)]
        public string LinkText { get; set; }

        [MaxLength(30)]
        public string LinkUrl { get; set; }
        
    }
}
