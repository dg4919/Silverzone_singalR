using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SilverzoneERP.Data
{
    class RolePermissionRepository:BaseRepository<RolePermission>,IRolePermissionRepository
    {
        public RolePermissionRepository(SilverzoneERPContext context) : base(context) { }

        public RolePermission GetByRoleId(long RoleId)
        {
            return FindBy(x=>x.RoleId==RoleId && x.Status == true).FirstOrDefault();
        }
        public RolePermission GetById(long Id)
        {
            return FindBy(x => x.Id == Id && x.Status == true).FirstOrDefault();
        }
        public dynamic Get()
        {
            try
            {
                var data = from rp in _dbContext.RolePermissions.Where(x => x.Status == true)
                           select new
                           {
                               rp.RoleId,
                               rp.Role.RoleName,
                               rp.Role.RoleDescription,
                               rp.FormId,
                               rp.FormManagement.FormName,
                               Permission =new {rp.Add,rp.Edit,rp.Delete,rp.Read,rp.Print}
                           } into newList
                           group newList by newList.RoleId into g
                           select new
                           {
                               g.FirstOrDefault().RoleId,
                               g.FirstOrDefault().RoleName,
                               g.FirstOrDefault().RoleDescription,
                               Forms = g.Select(x => new { x.FormId, x.FormName, Permission = JObject.Parse("{\"Add\":true,\"Edit\":true,\"Read\":true,\"Print\":true,\"Delete\":true}") })
                           };
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
