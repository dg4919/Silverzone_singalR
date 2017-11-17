using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IRoleRepository: IRepository<Role>
    {
        dynamic Get();
        Role GetById(long Id);
        Role GetByName(string Name);
        Role GetByName_Id(string Name, long id);
        bool role_isActive(int id);
    }
}
