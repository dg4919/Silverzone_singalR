using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class UserPermissionRepository:BaseRepository<UserPermission>,IUserPermissionRepository
    {
        public UserPermissionRepository(SilverzoneERPContext context) : base(context) { }

        public UserPermission GetById(long Id)
        {
            return FindBy(x=>x.Id==Id && x.Status == true).FirstOrDefault();
        }
        //public IEnumerable<UserPermission> GetByUserId(int UserId)
        //{
        //    return FindBy(x => x.UserId == UserId);
        //}
    }
}
