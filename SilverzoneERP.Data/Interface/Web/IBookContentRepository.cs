using System.Collections.Generic;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IBookContentRepository : IRepository<BookContent>
    {
        BookContent GetById(int id);
        BookContent GetByName(string name);
        IEnumerable<BookContent> GetByStatus(bool status);
    }
}
