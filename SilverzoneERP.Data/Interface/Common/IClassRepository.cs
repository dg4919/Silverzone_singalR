using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IClassRepository : IRepository<Class>
    {
        dynamic Get();
        Class Get(long ClassId);      
        bool Exists(string ClassName);
        bool Exists(long ClasssId,string ClassName);
    }    
}
