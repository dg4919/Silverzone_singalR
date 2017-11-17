using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class OlympiadList:Entity<long>
    {
        [MaxLength(50)]
        public string OlympiadName { get; set; }
       [DataType(DataType.Date)]
        public string FirstDate { get; set; }
        [DataType(DataType.Date)]
        public string SecondDate { get; set; }
        [DataType(DataType.Date)]
        public string LastDateOfRegistration { get; set; }
    }
}
