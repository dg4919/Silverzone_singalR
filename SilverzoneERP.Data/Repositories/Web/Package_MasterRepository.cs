using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class Package_MasterRepository : BaseRepository<Package_Master>, IPackage_MasterRepository
    {
        public Package_MasterRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public override IQueryable<Package_Master> GetAll()
        {
            return FindBy(x => x.Status == true);
        }

    }
}
