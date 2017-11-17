using SilverzoneERP.Data;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
     [Authorize]
    public class formManagementController : ApiController
    {             
        IRolePermissionRepository _rolePermissionRepository;
        IFormManagementRepository _formManagementRepository;
        IUserPermissionRepository _userPermissionRepository;
        //*****************  Constructor********************************

        public formManagementController( IRolePermissionRepository _rolePermissionRepository, IFormManagementRepository _formManagementRepository, IUserPermissionRepository _userPermissionRepository)
        {     
            this._rolePermissionRepository = _rolePermissionRepository;
            this._formManagementRepository = _formManagementRepository;
            this._userPermissionRepository = _userPermissionRepository;
        }

        //[HttpGet]
        //public IHttpActionResult GetFormUrlByUserId(long UserId)
        //{
        //    try
        //    {
        //        var headerList = (from frm in _formManagementRepository.FindBy(x => x.FormParentId == null && x.Status == true)
        //                          select new { frm.Id, frm.FormName }).ToList();

        //        var data = from ur in _userRoleRepository.FindBy(x => x.UserId == UserId && x.Status == true)
        //                   join rp in _rolePermissionRepository.FindBy(x => x.Status == true)
        //                   on ur.RoleId equals rp.RoleId

        //                   join up in _userPermissionRepository.FindBy(x => x.Status == true && x.UserId == UserId)
        //                   on rp.FormId equals up.FormId
        //                   into Details
        //                   from a in Details.DefaultIfEmpty(new UserPermission())

        //                   select new
        //                   {
        //                       header = headerList.Where(x => x.Id == rp.FormManagement.FormParentId).FirstOrDefault().FormName,
        //                       rp.FormManagement.Id,
        //                       rp.FormManagement.FormName,
        //                       rp.FormManagement.FormUrl,
        //                       rp.FormManagement.FormOrder,
        //                       Permission = JObject.Parse(a.Permission == null ? rp.Permission : a.Permission)
        //                   }
        //                   into newList
        //                   group newList by newList.header into g
        //                   select new
        //                   {
        //                       Header = g.Key,
        //                       Forms = g.Select(x => new { x.Id, x.FormName, x.FormUrl, Permission = x.Permission,x.FormOrder }).OrderBy(x=>x.FormOrder),
        //                   };


        //        return Ok(new { result = data });
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }           
        //}       

        [HttpGet]
        public IHttpActionResult GetFormGroupWise()
        {

            var data = from frm in _formManagementRepository.FindBy(x => x.Status == true && x.FormParentId == null).OrderBy(x=>x.FormOrder)
                       select new {
                           Header=frm.FormName,
                           Forms = frm.ChildFormManagement.Where(x=>x.FormName.ToLower()!="divider"&& x.Status==true ).OrderBy(x=>x.FormOrder).Select(x => new { FormId=x.Id, x.FormName, Permission = new { Add = false, Edit = false, Read = false, Print = false, Delete = false } })
                       };                      
            return Ok(new { result = data });
        }
    }
}
