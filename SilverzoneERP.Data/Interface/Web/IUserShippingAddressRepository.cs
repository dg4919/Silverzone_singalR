using System.Collections.Generic;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IUserShippingAddressRepository:IRepository<UserShippingAddress>
    {
        UserShippingAddress GetById(long id);
        IEnumerable<UserShippingAddress> GetByUserId(int id);
    }
}
