using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data.Repositories
{
    public class MasterAcademicYearRepository:BaseRepository<MasterAcademicYear>,IMasterAcademicYearRepository
    {
        public MasterAcademicYearRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public MasterAcademicYear GetById(int  id)
        {
            return _dbset.Where(x => x.Id == id).SingleOrDefault();
        }

        public  MasterAcademicYear  GetByYear(string Year)
        {
            return FindBy(x => x.CurrentAcademicYear == Year).SingleOrDefault();
        }
    }
}
