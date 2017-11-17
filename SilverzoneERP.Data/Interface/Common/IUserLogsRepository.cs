using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IUserLogsRepository:IRepository<UserLogs>
    {
        UserLogs GetById(int id);
        IEnumerable<UserLogs> GetByDate(DateTime sdate);
        IEnumerable<UserLogs> GetByUserId(int id);
    }
}
