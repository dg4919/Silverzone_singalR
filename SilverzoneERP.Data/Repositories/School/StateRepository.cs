using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;

namespace SilverzoneERP.Data
{
    public class StateRepository:BaseRepository<State>,IStateRepository
    {
        public StateRepository(SilverzoneERPContext context) : base(context) { }

        public bool Exists(string StateName, string StateCode, long CountryId)
        {
            return _dbset.FirstOrDefault(x => x.StateName.Trim().ToLower() == StateName.Trim().ToLower() && x.StateCode.Trim().ToLower() == StateCode.Trim().ToLower() && x.CountryId==CountryId) == null ? false : true;
        }
        public bool Exists(long StateId, string StateName, string StateCode, long CountryId)
        {
            return _dbset.FirstOrDefault(x =>x.Id!=StateId &&x.StateName.Trim().ToLower() == StateName.Trim().ToLower() && x.StateCode.Trim().ToLower() == StateCode.Trim().ToLower() && x.CountryId == CountryId) == null ? false : true;
        }
        public bool Exists(long StateId, string StateCode, long CountryId)
        {
            return _dbset.FirstOrDefault(x => x.Id != StateId  && x.StateCode.Trim().ToLower() == StateCode.Trim().ToLower() && x.CountryId == CountryId) == null ? false : true;
        }
        public State GetById(long StateId)
        {
            return _dbset.FirstOrDefault(x => x.Id == StateId);
        }
    }
}
