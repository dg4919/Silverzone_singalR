using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IBundleDetailRepository : IRepository<BookBundleDetails>
    {
        BookBundleDetails GetById(int id);
        //BookBundle GetByBookId(int id);
    }
}
