using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class SchoolGroupRepository : BaseRepository<SchoolGroup>, ISchoolGroupRepository
    {
        public SchoolGroupRepository(SilverzoneERPContext context) : base(context) { }

        public SchoolGroup Get(long SchoolGroupId)
        {
            return _dbset.FirstOrDefault(x => x.Id == SchoolGroupId);
        }
        public IList<SchoolGroup> Get(bool Status)
        {
            return _dbset.Where(x => x.Status == Status).ToList();
        }
        public bool Exists(string SchoolGroupName)
        {
            return _dbset.FirstOrDefault(x => x.SchoolGroupName == SchoolGroupName) == null ? false : true;
        }
        public bool Exists(long SchoolGroupId, string SchoolGroupName)
        {
            return _dbset.FirstOrDefault(x => x.Id != SchoolGroupId && x.SchoolGroupName == SchoolGroupName) == null ? false : true;
        }
    }
}
