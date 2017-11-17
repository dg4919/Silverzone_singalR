using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class EventYearClassRepository : BaseRepository<EventYearClass>, IEventYearClassRepository
    {
        public EventYearClassRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

       public IList<EventYearClass> Get(long EventYearId)
        {
            return _dbset.Where(x => x.EventYearId == EventYearId).ToList();
        }
    }
}
