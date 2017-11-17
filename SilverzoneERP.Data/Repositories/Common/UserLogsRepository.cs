using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class UserLogsRepository:BaseRepository<UserLogs>,IUserLogsRepository
    {
        public UserLogsRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public UserLogs GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
         public IEnumerable<UserLogs>  GetByDate(DateTime sdate)
        {
            return _dbset.Where(x => x.Login_DateTime == sdate).AsEnumerable();
        }

        public IEnumerable<UserLogs> GetByUserId(int id)
        {
            return _dbset.Where(x => x.UserId == id).AsEnumerable();
        }
    }
}
