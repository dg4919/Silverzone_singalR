using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class ProfileRepository:BaseRepository<Profile>,IProfileRepository
    {
        public ProfileRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public Profile GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Profile> GetByStatus(bool status)
        {
            return _dbset.Where(x => x.Status == status).AsEnumerable();
        }

        public bool isExist(string profileName)
        {
            return _dbset.Any(x => x.ProfileName.Equals(profileName));
        }

    }
}
