using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;
namespace SilverzoneERP.Data
{
    public class BundleDetailRepository : BaseRepository<BookBundleDetails>, IBundleDetailRepository
    {
        public BundleDetailRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookBundleDetails GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
