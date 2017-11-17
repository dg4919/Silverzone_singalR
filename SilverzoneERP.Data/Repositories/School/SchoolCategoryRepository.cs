using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class SchoolCategoryRepository : BaseRepository<SchoolCategory>, ISchoolCategoryRepository
    {
        public SchoolCategoryRepository(SilverzoneERPContext context) : base(context) { }

        public SchoolCategory Get(long CategoryId)
        {
            return _dbContext.SchoolCategories.FirstOrDefault(x => x.Id == CategoryId);
        }
        public bool Exists(string CategoryName)
        {
            return _dbContext.SchoolCategories.FirstOrDefault(x => x.CategoryName == CategoryName) == null ? false : true;
        }
        public bool Exists(long CategoryId, string CategoryName)
        {
            return _dbContext.SchoolCategories.FirstOrDefault(x => x.Id != CategoryId && x.CategoryName == CategoryName) == null ? false : true;
        }
        public IList<SchoolCategory> Get(bool Status)
        {
            return _dbset.Where(x => x.Status == true).ToList();
        }
    }
}
