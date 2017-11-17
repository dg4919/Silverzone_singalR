using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class EventRepository:BaseRepository<Event>,IEventRepository
    {
        public EventRepository(SilverzoneERPContext context) : base(context) { }
        
        public Event GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
        public Event GetByName(string name)
        {
            return FindBy(x => x.EventName == name).FirstOrDefault();
        }

        public Event GetByCode(string eventCode)
        {
            return FindBy(x => x.EventCode == eventCode).FirstOrDefault();
        }


        public IQueryable<Event> GetByStatus(bool status)
        {
            return FindBy(x => x.Status == status);
        }

        public bool is_eventExist(string eventName, string eventCode)
        {
            return _dbset.Any(x => x.EventCode.Equals(eventCode)
                                && x.EventName.Equals(eventName)
                                );
        }

        public dynamic GetAll_School()
        {
            return _dbset.Select(x => new {
                x.Id,
                x.EventName,
                x.SubjectName,
                x.EventCode,
                x.RowVersion,
                x.Status
            });
        }

        public bool Exists(string EventName, string EventCode,string SubjectName, out string Message)
        {
            if(_dbset.FirstOrDefault(x => x.EventName.Trim().ToLower() == EventName.Trim().ToLower()) != null)
            {
                Message = "Event Name already exists !";
                return true;
            }
            else if(_dbset.FirstOrDefault(x => x.EventCode.Trim().ToLower() == EventCode.Trim().ToLower()) != null)
            {
                Message = "Event Code already exists !";
                return true;
            }
            else if (_dbset.FirstOrDefault(x => x.SubjectName.Trim().ToLower() == SubjectName.Trim().ToLower()) != null)
            {
                Message = "Subject Name already exists !";
                return true;
            }
            Message = "";
            return false;
        }

        public bool Exists(long EventId,string EventName, string EventCode, string SubjectName, out string Message)
        {
            if (_dbset.FirstOrDefault(x => x.Id!= EventId && x.EventName.Trim().ToLower() == EventName.Trim().ToLower()) != null)
            {
                Message = "Event Name already exists !";
                return true;
            }
            else if (_dbset.FirstOrDefault(x => x.Id != EventId && x.EventCode.Trim().ToLower() == EventCode.Trim().ToLower()) != null)
            {
                Message = "Event Code already exists !";
                return true;
            }
            else if (_dbset.FirstOrDefault(x => x.Id != EventId && x.SubjectName.Trim().ToLower() == SubjectName.Trim().ToLower()) != null)
            {
                Message = "Subject Name already exists !";
                return true;
            }
            Message = "";
            return false;
        }

    }
}
