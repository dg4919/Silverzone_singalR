using SilverzoneERP.Entities.Models.Common;
using System;

namespace SilverzoneERP.Entities.Models
{
    public class UserLogs:Entity<long>
    {
        public long UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime Login_DateTime { get; set; }
        public string IPAddress { get; set; }
        public string Browser { get; set; }
        public string Location { get; set; }
    }
}
