using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IBannerRepository:IRepository<Banner>
    {
        Banner GetById(int id);
        IEnumerable<Banner> GetByStatus(bool status);
    }
}
