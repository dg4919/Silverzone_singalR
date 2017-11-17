using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IOlympiadListRepository:IRepository<OlympiadList>
    {
        OlympiadList GetById(int id);
       IEnumerable<OlympiadList> GetByStatus(bool status);
    }
}
