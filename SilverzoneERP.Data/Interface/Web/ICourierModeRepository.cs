using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface ICourierModeRepository : IRepository<CourierMode>
    {
        IQueryable<CourierMode> GetByCourierId(int CourierId);
    }
}
