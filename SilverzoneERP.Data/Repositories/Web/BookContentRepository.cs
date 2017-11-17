using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class BookContentRepository : BaseRepository<BookContent>, IBookContentRepository
    {
        public BookContentRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookContent GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

        public BookContent GetByName(string name)
        {
            return FindBy(x => x.Name == name).FirstOrDefault();
        }

        public IEnumerable<BookContent> GetByStatus(bool status)
        {
            return _dbset.Where(x => x.Status == status).AsEnumerable();
        }

        public void deleteWhere(IEnumerable<BookContent> contents)
        {
            // RemoveRange is attach with specific table and context clas
            _dbContext.Contents.RemoveRange(contents);
        }
    }
}
