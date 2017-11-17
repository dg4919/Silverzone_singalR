using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IRolePermissionRepository:IRepository<RolePermission>
    {
        RolePermission GetByRoleId(long RoleId);
        RolePermission GetById(long Id);
        dynamic Get();
    }
}
