using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class Subject:AuditableEntity<long>
    {
        [MaxLength(100)]
        public string Name { get; set; }
        
        public virtual ICollection<Book> Books { get; set; }

        public ICollection<classSubject> classSubjects { get; set; }
    }
}
