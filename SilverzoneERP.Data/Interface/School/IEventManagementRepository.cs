using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IEventManagementRepository : IRepository<EventManagement>
    {
        List<EventManagement> GetBySchoolId(long SchoolId);
        bool Exists(long SchoolId);
        EventManagement Get(long Id);
        dynamic Get_FavourOf();
        dynamic Get_DrawnOnBank();
    }
}
