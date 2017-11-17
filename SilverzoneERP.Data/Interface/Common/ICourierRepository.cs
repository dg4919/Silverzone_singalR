using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface ICourierRepository : IRepository<Courier>
    {
        IQueryable<Courier> GetActive();
        Courier Get(long CouriorId);
        bool Exists(string CouriorName);
        bool Exists(long CouriorId, string CouriorName);
    }
}
