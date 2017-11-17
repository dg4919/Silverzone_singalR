using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class Packet_BundleInfo : AuditableEntity<long>
    {
        public long PM_Id { get; set; }
        [ForeignKey("PM_Id")]
        public virtual Package_Master Package_Master { get; set; }

        public long dispatch_mId { get; set; }
        [ForeignKey("dispatch_mId")]
        public virtual Dispatch_Master Dispatch_Master { get; set; }

        public decimal Netwheight { get; set; }
    }
}
