using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class SchoolLog:Entity<long>
    {
        public long SchId { set; get; }
        public long CreatedBy { set; get; }
        public DateTime CreationDate { set; get; }

        [ForeignKey("CreatedBy")]
        public virtual ERPuser ERPuser { get; set; }

        [ForeignKey("SchId")]
        public virtual School School { get; set; }
    }
}
