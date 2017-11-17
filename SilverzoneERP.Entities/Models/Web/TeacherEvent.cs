using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class TeacherEvent : Entity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }

        public long EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}
