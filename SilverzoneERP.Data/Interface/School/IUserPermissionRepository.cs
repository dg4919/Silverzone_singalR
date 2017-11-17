using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IUserPermissionRepository:IRepository<UserPermission>
    {
        UserPermission GetById(long Id);
        //IEnumerable<UserPermission> GetByUserId(int UserId);
    }
}
