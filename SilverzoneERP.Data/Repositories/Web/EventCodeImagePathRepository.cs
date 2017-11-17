using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data.Repositories
{
    public class EventCodeImagePathRepository : BaseRepository<EventCodeImagePath>, IEventCodeImagePathRepository
    {
        public EventCodeImagePathRepository(SilverzoneERPContext dbcontext) : base(dbcontext)
        {
        }

        public EventCodeImagePath GeByEventCode(int id)
        {
            return FindBy(x => x.EventId == id).SingleOrDefault();
        }
    }
}
