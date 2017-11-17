using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IQueryRepository:IRepository<Query>
    {
        Query GetById(int id);
        IEnumerable<Query> GetByStatus(string status);
        IEnumerable<Query> GetByQueryDate(DateTime qdate);
        IEnumerable<Query> GetByCloseDate(DateTime cdate);
    }
}
