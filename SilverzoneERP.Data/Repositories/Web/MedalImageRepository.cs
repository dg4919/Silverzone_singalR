using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class MedalImageRepository:BaseRepository<MedalImage>,IMedalImageRepository
    {
        public MedalImageRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public MedalImage GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }

        public  IEnumerable<MedalImage> GetByStatus(bool status)
        {
            return FindBy(x => x.Status == status).AsEnumerable();
        }
    }
}
