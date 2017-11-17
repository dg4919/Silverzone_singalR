using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface IEventRepository:IRepository<Event>
    {
        Event GetById(int id);
        Event GetByName(string Name);
        Event GetByCode(string eventCode);
        IQueryable<Event> GetByStatus(bool status);

        bool is_eventExist(string eventName, string eventCode);

        dynamic GetAll_School();
        bool Exists(string EventName,string EventCode, string SubjectName, out string Message);
        bool Exists(long EventId,string EventName, string EventCode, string SubjectName, out string Message);
    }
}
