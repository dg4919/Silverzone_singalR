using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IBlackListedRepository : IRepository<BlackListedSchool>
    {
       List<BlackListedSchool> GetBySchoolId(long Id);
        bool IsChange(long SchoolId,bool IsBlocked);
        dynamic Get_Latest_BlackListd(long SchoolId);
    }
}
