using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
   public  class MetaTag:Entity<long>
    {
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(50)]
        public string Link { get; set;}

        [MaxLength(200)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Keyword { get; set; }
        
    }
}
