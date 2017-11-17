using System.Collections.Generic;
using System;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class CoOrdinatorRepository : BaseRepository<CoOrdinator>, ICoOrdinatorRepository
    {
        public CoOrdinatorRepository(SilverzoneERPContext context) : base(context) { }

        public List<CoOrdinator> GetByEventCoOrdId(long EventManagementId)
        {
            try
            {
                return _dbContext.CoOrdinators.Where(x=>x.EventManagementId== EventManagementId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }                       
        }
    }
}
