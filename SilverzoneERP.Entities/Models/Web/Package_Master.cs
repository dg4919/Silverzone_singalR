using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class Package_Master : AuditableEntity<long>
    {
        public string Name { get; set; }
        public decimal wheight { get; set; }
    }
}
