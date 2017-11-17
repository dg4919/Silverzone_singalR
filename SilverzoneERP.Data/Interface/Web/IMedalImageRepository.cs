using SilverzoneERP.Entities.Models;
using System.Collections.Generic;


namespace SilverzoneERP.Data
{
    public interface IMedalImageRepository:IRepository<MedalImage>
    {
        MedalImage GetById(int id);
        IEnumerable<MedalImage> GetByStatus(bool status);
    }
}
