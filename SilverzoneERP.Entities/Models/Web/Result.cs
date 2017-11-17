using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class Result:Entity<long>
    {
        [MaxLength(15)]
        public string SchCode { get; set; }
       
        [MaxLength(2)]
        public string RollNo { get; set; }

        [MaxLength(4)]
        public string Class { get; set; }

        [MaxLength(1)]
        public string Sections { get; set; }

        public decimal TotMarks { get; set; }

        public int ClassRank { get; set; }
        public int StateRank { get; set; }
        public int AllIndiaRank { get; set; }
        [MaxLength(12)]
        public string NIORollNo { get; set; }

        [MaxLength(100)]
        public string StudName { get; set; }

        public decimal RawScore { get; set; }
        public bool SecondLevelEligible { get; set; }
        public bool Medal { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public string EventYear { get; set; }
        public resultLevel Level { get; set; } 
    }
}
