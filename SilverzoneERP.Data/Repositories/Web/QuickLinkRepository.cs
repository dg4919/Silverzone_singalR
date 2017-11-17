using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class QuickLinkRepository:BaseRepository<QuickLink>,IQuickLinkRepository
    {
        public QuickLinkRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
        public QuickLink  GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<QuickLink> GetByStatus(bool status)
        {
            return _dbset.Where(x => x.Status == status).AsEnumerable();
        }
    }
}
