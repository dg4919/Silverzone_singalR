using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface ISchoolCategoryRepository : IRepository<SchoolCategory>
    {
        SchoolCategory Get(long CategoryId);
        bool Exists(string CategoryName);
        bool Exists(long CategoryId, string CategoryName);
        IList<SchoolCategory> Get(bool Status);
    }
}
