using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class UserShippingAddressRepository:BaseRepository<UserShippingAddress>,IUserShippingAddressRepository
    {
        public UserShippingAddressRepository(SilverzoneERPContext dbcontext):base(dbcontext){ }
        public UserShippingAddress GetById(long id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
        public IEnumerable<UserShippingAddress> GetByUserId(int id)
        {
            return FindBy(x => x.UserId == id && x.Status == true).AsEnumerable();
        }
    }
}
