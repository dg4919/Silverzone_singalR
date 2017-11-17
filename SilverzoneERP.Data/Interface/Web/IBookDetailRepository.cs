using System.Collections.Generic;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IBookDetailRepository:IRepository<BookDetail>
    {
        BookDetail GetById(int id);
        IEnumerable<BookDetail> GetByBookId(int id);
    }
}
