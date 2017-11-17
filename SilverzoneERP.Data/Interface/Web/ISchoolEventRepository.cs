using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface ISchoolEventRepository:IRepository<SchoolEvent>
    {
        SchoolEvent GetById(int id);
        IEnumerable<SchoolEvent> GetBySchCode(long SchCode);
        SchoolEvent GetByYear(string year);
    }
}
