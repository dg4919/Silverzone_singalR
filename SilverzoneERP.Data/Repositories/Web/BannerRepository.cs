using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class BannerRepository:BaseRepository<Banner>,IBannerRepository
    {
        public BannerRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public Banner GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
        public IEnumerable<Banner> GetByStatus(bool stat)
        {
            return _dbset.Where(x => x.Status == stat).AsEnumerable();
        }
    }
}
