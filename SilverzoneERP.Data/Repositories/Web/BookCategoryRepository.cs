using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class BookCategoryRepository : BaseRepository<BookCategory>, IBookCategoryRepository
    {
        public BookCategoryRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookCategory GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

        // ICollection have Count property > but IEnumerable has not
        public ICollection<BookCategory> GetByName(string Name)
        {
            //return FindBy(x => x.Name == Name);            // match exact wrd
            //return FindBy(x => x.Name.Contains(Name));    // use as like by in SQL

            // trim use to remove extra space + match with lower case
            return FindBy(x => x.Name.ToLower().Trim() == Name.ToLower().Trim()).ToList();    // use as like by in SQL
        }

        public IEnumerable<BookCategory> GetByNameAndStatus(string name,bool status)
        {
            // Extension methods of IQueryable<T> like > count, Add method will be show only in repostory > not in API
            return FindBy(x => x.Name.Contains(name) && x.Status == status).AsEnumerable();
        }

        public IEnumerable<BookCategory> GetByStatus(bool status)
        {
            return _dbset.Where(x => x.Status == status).AsEnumerable();
        }
        
        public bool Iscategory_Exist(string Name, int Id)
        {
            return _dbset.Any(x => x.Name.ToLower().Trim() == Name.ToLower().Trim() && x.Id != Id);
        }

    }
}
