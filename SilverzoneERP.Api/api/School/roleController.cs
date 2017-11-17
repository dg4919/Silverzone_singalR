using SilverzoneERP.Data;
using System;
using System.Linq;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class roleController : ApiController
    {
        IRoleRepository _roleRepository;
        IUserRepository _userRepository;
        IRolePermissionRepository _rolePermissionRepository;
        IFormManagementRepository _formManagementRepository;
        public roleController(IRoleRepository _roleRepository, IRolePermissionRepository _rolePermissionRepository, IFormManagementRepository _formManagementRepository, IUserRepository _userRepository)
        {
            this._roleRepository = _roleRepository;
            this._rolePermissionRepository = _rolePermissionRepository;
            this._formManagementRepository= _formManagementRepository;
            this._userRepository = _userRepository;
        }

        public IHttpActionResult Get()
        {
            try
            {
                var data = _roleRepository.Get();
                return Ok(new { result = data });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IHttpActionResult GetRolePermission()
        {
            try
            {
                var data = _getRolePermission();
                return Ok(new { result = data });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //[HttpPost]
        //public IHttpActionResult userRole(UserRoleViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (multiSelect _user in model.Users)
        //        {

        //            var _userRole = userRoleRepository.FindBy(x => x.UserId == _user.Id).FirstOrDefault();
        //            if (_userRole == null)
        //            {
        //                userRoleRepository.Create(new UserRole
        //                {
        //                    UserId = _user.Id,
        //                    RoleId = model.RoleId,
        //                    Status = true
        //                });
        //            }
        //            else
        //            {
        //                _userRole.RoleId = model.RoleId;
        //                _userRole.Status = true;
        //                userRoleRepository.Update(_userRole);
        //            }
        //        }
        //        return Ok(new { result = "Success", message = "User Role created successfully!" });
        //    }

        //    return Ok(new { result = "error", message = "error" });
        //}

        [HttpPost]
        public IHttpActionResult Create_Update(RolePermissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";
                if (model.Id == 0)
                {
                    var role = _roleRepository.FindBy(x => x.RoleName.ToLower() == model.RoleName.ToLower()).FirstOrDefault();
                    if (role == null)
                    {
                        var rolePermissionList = _rolePermissionRepository.FindBy(x => x.Id == model.Id);
                        foreach (var item in rolePermissionList)
                        {
                            _rolePermissionRepository.Delete(item);
                        }
                        //Create role
                        model.Id = _roleRepository.Create(new Role
                        {
                            RoleName = model.RoleName,
                            RoleDescription = model.RoleDescription,
                            Status = true
                        }).Id;
                        msg = "Role created Successfully!";
                    }
                    else
                    {
                        msg = "Role already exists!";
                    }
                }
                else
                {
                    var role = _roleRepository.FindBy(x => x.Id == model.Id).FirstOrDefault();
                    if (role != null)
                    {
                        var _role = _roleRepository.FindBy(x => x.Id != model.Id && x.RoleName.ToLower() == model.RoleName.ToLower()).FirstOrDefault();
                        if (_role == null)
                        {
                            role.Status = true;
                            role.RoleDescription = model.RoleDescription;
                            _roleRepository.Update(role);
                            model.Id = role.Id;
                            msg = "Successfully role updated !";
                        }
                        else
                            msg = "Role already exists!";
                    }

                    DeleteRolePermission(model.Id);
                }
                //Create Role Permission on Form
                foreach (Forms item in model.Forms)
                {
                    _rolePermissionRepository.Create(new RolePermission
                    {
                        RoleId = model.Id,                        
                        Add=item.Permission.Add,
                        Edit = item.Permission.Edit,
                        Delete = item.Permission.Delete,
                        Read = item.Permission.Read,
                        Print = item.Permission.Print,
                        FormId = item.FormId,
                        Status = true
                    });
                }
                return Ok(new { result = "Success", message = msg });
            }
            return Ok(new { result = "error", message = "error" });
        }

        private void DeleteRolePermission(long Id)
        {
            var rolePermissionList = _rolePermissionRepository.FindBy(x => x.RoleId == Id);
            _rolePermissionRepository.DeleteWhere(rolePermissionList);
            _rolePermissionRepository.Save();
        }


        private dynamic _getRolePermission()
        {
            try
            {
                var headerList = from frm in _formManagementRepository.FindBy(x => x.FormParentId == null && x.Status == true)
                                  select new { frm.Id, frm.FormName };


                var data = from rp in _rolePermissionRepository.FindBy(x => x.Status == true ).OrderByDescending(x => x.Role.UpdationDate)
                           select new
                           {
                               rp.RoleId,
                               rp.Role.RoleName,
                               rp.Role.RoleDescription,
                               rp.FormId,
                               Header = headerList.Where(x => x.Id == rp.FormManagement.FormParentId).FirstOrDefault().FormName,
                               rp.FormManagement.FormName,
                               Permission=new {rp.Add,rp.Edit,rp.Delete,rp.Read,rp.Print}
                           } into newList
                           group newList by newList.RoleId into g
                           select new
                           {
                               g.FirstOrDefault().RoleId,
                               g.FirstOrDefault().RoleName,
                               g.FirstOrDefault().RoleDescription,
                               Forms = g.ToList().Select(x => new
                               {
                                   x.Header,                                   
                                   x.FormId,
                                   x.FormName,
                                   x.Permission 
                               })
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
