using System.Collections.Generic;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IBookCategoryRepository : IRepository<BookCategory>
    {
        BookCategory GetById(int id);
        IEnumerable<BookCategory> GetByNameAndStatus(string name, bool status);
        IEnumerable<BookCategory> GetByStatus(bool status);

        ICollection<BookCategory> GetByName(string Name);
        bool Iscategory_Exist(string Name, int Id);
    }
}
