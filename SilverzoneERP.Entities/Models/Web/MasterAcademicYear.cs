using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class MasterAcademicYear:Entity<long>
    {
        [MaxLength(7)]
        public string CurrentAcademicYear { get; set; }
        [MaxLength(7)]
        public string LastAcademicYear { get; set; }
        [MaxLength(80)]
        public  string CurrentEventCodes { get; set; }
        [MaxLength(80)]
        public string LastEventCodes { get; set; }
    }
}
