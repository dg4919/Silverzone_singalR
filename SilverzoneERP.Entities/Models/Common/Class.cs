using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;

namespace SilverzoneERP.Entities.Models
{
    public class Class : AuditableEntity<long>
    {
       public string className { get; set; }

        public virtual ICollection<classSubject> classSubjects { get; set; }
    }

}
