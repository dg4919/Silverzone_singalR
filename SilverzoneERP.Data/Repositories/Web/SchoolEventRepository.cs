using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data.Repositories
{
    public class SchoolEventRepository:BaseRepository<SchoolEvent>,ISchoolEventRepository
    {
        public SchoolEventRepository(SilverzoneERPContext dbcontext):base(dbcontext){}
        public SchoolEvent GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
        public IEnumerable<SchoolEvent> GetBySchCode(long schcode)
        {
            return _dbset.Where(x => x.School.SchCode == schcode).AsEnumerable();
        }
        public SchoolEvent GetByYear(string year)
        {
            return FindBy(x => x.EventYear == year).FirstOrDefault();
        }
    }
}
