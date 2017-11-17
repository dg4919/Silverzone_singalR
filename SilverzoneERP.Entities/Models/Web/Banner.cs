using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class Banner:Entity<long>
    {
        public string ImageName { get; set; }
        public string Caption { get; set; }
        public string Link { get; set; }
    }
}
