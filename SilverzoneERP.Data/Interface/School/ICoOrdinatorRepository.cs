using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface ICoOrdinatorRepository : IRepository<CoOrdinator>
    {
       List<CoOrdinator> GetByEventCoOrdId(long EventManagementId);
    }
}
