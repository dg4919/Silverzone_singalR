using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IEventYearRepository : IRepository<EventYear>
    {
        EventYear Get(long EventYearId);      
        bool Exists(long EventId,int EventYear);
        bool Exists(long EventYearId, long EventId, int EventYear);
        dynamic Get(bool IsStatus = false);
        dynamic Get(int EventYear, bool Status, string EventCode = null);
    }    
}
