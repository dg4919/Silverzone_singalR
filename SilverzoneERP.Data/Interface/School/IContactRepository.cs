using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IContactRepository : IRepository<Contact>
    {
        List<Contact> GetBySchoolId(long SchoolId);
    }
}
