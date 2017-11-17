using System.Collections.Generic;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface ISubjectRepository:IRepository<Subject>
    {
        Subject GetById(int id);
        Subject GetByName(string name);
        IEnumerable<Subject> GetByStatus(bool status);
    }
}
