using System.Collections.Generic;
using System;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class EventManagementRepository : BaseRepository<EventManagement>, IEventManagementRepository
    {
        public EventManagementRepository(SilverzoneERPContext context) : base(context) { }

        public List<EventManagement> GetBySchoolId(long SchoolId)
        {
            try
            {
                return _dbContext.EventManagements.Where(x => x.SchId == SchoolId && x.EventManagementYear == DateTime.Now.Year).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Exists(long SchoolId)
        {
            return _dbset.Any(x => x.SchId == SchoolId && x.EventManagementYear == DateTime.Now.Year);
        }
        public EventManagement Get(long Id)
        {
            return _dbset.FirstOrDefault(x => x.Id == Id);
        }

        public dynamic Get_FavourOf()
        {
            var data= _dbContext.InFavourOfs.Where(x => x.Status == true).ToList()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    DipositOnBanks = x.DipositOnBank.Where(d => d.Status == true).ToList().Select(d => new
                    {
                        d.Id,
                        d.BankName,
                        d.AccountNo
                    })
                });
            return data;
        }
        public dynamic Get_DrawnOnBank()
        {
            var data = _dbContext.DrownOnBanks.Where(x => x.Status == true).ToList()
                .Select(x => new
                {
                    x.Id,
                    x.BankName                    
                });
            return data;
        }
    }
}
