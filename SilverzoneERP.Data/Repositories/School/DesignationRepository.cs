using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public class DesignationRepository : BaseRepository<Designation>, IDesignationRepository
    {       
        public DesignationRepository(SilverzoneERPContext _dbContext) : base(_dbContext) { }

        public bool Exists(string DesgName)
        {
            return _dbset.FirstOrDefault(x => x.DesgName.Trim().ToLower() == DesgName.Trim().ToLower()) == null ? false : true;
        }
        public bool Exists(long DesgId, string DesgName)
        {
            return _dbset.FirstOrDefault(x => x.Id != DesgId && x.DesgName.Trim().ToLower() == DesgName.Trim().ToLower() ) == null ? false : true;
        }
        public Designation Get(long DesgId)
        {
            return _dbset.FirstOrDefault(x => x.Id == DesgId);
        }
        public IList<Designation> Get(bool Status)
        {
            return _dbset.Where(x => x.Status == true).ToList();
        }
    }
}
