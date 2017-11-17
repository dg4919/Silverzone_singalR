using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class Book:AuditableEntity<long>
    {
        [MaxLength(100)]
        public string Title { get; set; }
        public string BookImage { get; set; }
        [MaxLength(100)]
        public string ISBN { get; set; }
        [MaxLength(100)]
        public string Publisher { get; set; }
        [MaxLength(100)]
        public string Edition { get; set; }
        public int Pages { get; set; }
        public decimal Weight { get; set; }
        public decimal Price { get; set; }

        public bool in_Stock { get; set; }

        public long ReorderLevel { get; set; }

        public long Title_Mid { get; set; }
        [ForeignKey("Title_Mid")]
        public virtual ItemTitle_Master ItemTitle_Master { get; set; }

        public virtual BookDetail BookDetails { get; set; }
        public virtual ICollection<BookContent> BookContents { get; set; }
        public virtual ICollection<BookReview> BookReviews { get; set; }
        
    }
}
