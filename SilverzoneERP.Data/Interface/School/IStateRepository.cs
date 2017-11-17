using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IStateRepository:IRepository<State>
    {
        bool Exists(string StateName,string StateCode,long CountryId);
        bool Exists(long StateId,string StateName, string StateCode, long CountryId);
        bool Exists(long StateId, string StateCode, long CountryId);
        State GetById(long StateId);        
    }
}
