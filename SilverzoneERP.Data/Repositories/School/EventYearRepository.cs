using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class EventYearRepository : BaseRepository<EventYear>, IEventYearRepository
    {
        public EventYearRepository(SilverzoneERPContext context) : base(context) { }

        public EventYear Get(long EventYearId)
        {
            return _dbContext.EventYears.FirstOrDefault(x => x.Id == EventYearId);
        }
        public bool Exists(long EventId, int EventYear)
        {
            return _dbContext.EventYears.FirstOrDefault(x => x.Event_Year == DateTime.Now.Year && x.EventId == EventId && x.Event_Year == EventYear) == null ? false : true;
        }
        public bool Exists(long EventYearId, long EventId, int EventYear)
        {
            return _dbContext.EventYears.FirstOrDefault(x => x.Id != EventYearId && x.EventId == EventId && x.Event_Year == EventYear) == null ? false : true;
        }
        public dynamic Get(bool IsStatus=false)
        {
            if(IsStatus)
            {
                return _dbset.Where(x=>x.Status==true).OrderByDescending(x => x.UpdationDate)
                .Select(x => new
                {
                    EventYearId = x.Id,
                    EventYear = x.Event_Year,
                    x.EventId,
                    x.Event.EventName,
                    x.Event.EventCode,
                    x.EventFee,
                    x.RetainFee,    
                    Class=x.EventYearClass.Select(c=>new { c.ClassId, ClassName=c.Class.className, IsChecked =true}),               
                    x.RowVersion,
                    x.Status
                });
            }
            else
            {
                return _dbset.OrderByDescending(x => x.UpdationDate)
                .Select(x => new
                {
                    EventYearId = x.Id,
                    EventYear = x.Event_Year,
                    x.EventId,
                    x.Event.EventName,
                    x.Event.EventCode,
                    x.EventFee,
                    x.RetainFee,
                    Class = x.EventYearClass.Select(c => new { c.ClassId, ClassName = c.Class.className, IsChecked = true }),
                    x.RowVersion,
                    x.Status
                });
            }            
        }

        public dynamic Get(int EventYear, bool Status, string EventCode = null)
        {
            if (EventCode == null)
            {
                return _dbset.Where(x => x.Event_Year == EventYear && x.Status == Status).OrderByDescending(x => x.UpdationDate)
               .Select(x => new
               {
                   EventYearId = x.Id,
                   EventYear = x.Event_Year,
                   x.EventId,
                   x.Event.EventName,
                   x.Event.EventCode,
                   x.Event.SubjectName,
                   x.EventFee,
                   x.RetainFee,
                   Class = x.EventYearClass.Select(c => new { c.ClassId, ClassName = c.Class.className }),
                   x.RowVersion,
                   x.Status
               });
            }
            else
            {
                return _dbset.Where(x => x.Event_Year == EventYear && x.Event.EventCode.ToLower()==EventCode.ToLower() && x.Status == Status).OrderByDescending(x => x.UpdationDate)
               .Select(x => new
               {
                   EventYearId = x.Id,
                   EventYear = x.Event_Year,
                   x.EventId,
                   x.Event.EventName,
                   x.Event.EventCode,
                   x.EventFee,
                   x.RetainFee,
                   Class = x.EventYearClass.Select(c => new { c.ClassId, ClassName = c.Class.className }),
                   x.RowVersion,
                   x.Status
               }).FirstOrDefault();
            }
        }
    }
}
