using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class EventCodeImagePath:Entity<long>
    {
        public int EventId { get; set; }
        [MaxLength(50)]
        public string EventImagePath { get; set; }
    }
}
