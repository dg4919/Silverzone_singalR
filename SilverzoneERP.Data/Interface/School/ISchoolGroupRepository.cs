using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface ISchoolGroupRepository : IRepository<SchoolGroup>
    {
        SchoolGroup Get(long SchoolGroupId);
        IList<SchoolGroup> Get(bool Status);
        bool Exists(string SchoolGroupName);
        bool Exists(long SchoolGroupId, string SchoolGroupName);
    }
}
