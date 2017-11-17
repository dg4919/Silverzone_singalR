using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class BookDetailRepository:BaseRepository<BookDetail>,IBookDetailRepository
    {
        public BookDetailRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookDetail GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<BookDetail> GetByBookId(int id)
        {
            return _dbset.Where(x => x.Id == id).AsEnumerable();
        }
    

    }
}
