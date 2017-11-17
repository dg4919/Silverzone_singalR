using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IQuickLinkRepository:IRepository<QuickLink>
    {
        QuickLink GetById(int id);
        IEnumerable<QuickLink> GetByStatus(bool status);
    }
}
