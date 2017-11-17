using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;

namespace SilverzoneERP.Data
{
    class RoleRepository:BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(SilverzoneERPContext context) : base(context) { }

        public dynamic Get()
        {
            try
            {
                var data = from r in _dbContext.Roles.Where(x => x.Status == true)
                           select new
                           {
                               RoleId = r.Id,
                               r.RoleName,
                               r.RoleDescription
                           };
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Role GetById(long Id)
        {
            return FindBy(x => x.Id == Id && x.Status == true).SingleOrDefault();
        }
        public Role GetByName(string Name)
        {
            return FindBy(x => x.RoleName == Name && x.Status == true).FirstOrDefault();
        }
        public Role GetByName_Id(string Name, long id)
        {
            return FindBy(x => x.RoleName == Name && x.Id != id && x.Status == true).FirstOrDefault();
        }

        public bool role_isActive(int id)
        {
            return _dbset.Any(x => x.Id == id && x.Status == true);
        }
    }
}
