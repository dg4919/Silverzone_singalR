using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IOlympiadsRepository:IRepository<Olympiads>
    {
        Olympiads GetById(int id);
        IEnumerable<Olympiads> GetByStatus(bool status);
    }
}
