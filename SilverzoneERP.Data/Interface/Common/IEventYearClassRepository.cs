using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IEventYearClassRepository : IRepository<EventYearClass>
    {
        IList<EventYearClass> Get(long EventYearId);
    }    
}
