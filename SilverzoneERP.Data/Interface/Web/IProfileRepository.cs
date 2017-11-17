using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IProfileRepository:IRepository<Profile>
    {
        Profile GetById(int id);
        IEnumerable<Profile> GetByStatus(bool status);
        bool isExist(string profileName);
    }
}
