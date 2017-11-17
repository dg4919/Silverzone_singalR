using System.Web.Http;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Data;
using System;
using System.Linq;
using SilverzoneERP.Entities.ViewModel.School;

namespace SilverzoneERP.Api.api.School
{
    public class userController : ApiController
    {
        IUserRepository userRepository;
        IRoleRepository roleRepository;
        IRolePermissionRepository rolePermissionRepository;
        IFormManagementRepository formManagementRepository;
        IUserPermissionRepository userPermissionRepository;
        //*****************  Constructor********************************

        public userController(IUserRepository _userRepository, IRoleRepository _roleRepository, IRolePermissionRepository _rolePermissionRepository, IFormManagementRepository _formManagementRepository,IUserPermissionRepository _userPermissionRepository)
        {
            userRepository = _userRepository;
            roleRepository = _roleRepository;
            rolePermissionRepository=_rolePermissionRepository;
            formManagementRepository = _formManagementRepository;
            userPermissionRepository = _userPermissionRepository;
        }

        #region GET       
        //Use
        [HttpGet]
        public IHttpActionResult GetUserSummary(int UserId, int RoleId, int StartIndex, int Limit)
        {
            try
            {
                if (UserId == 0 && RoleId == 0)
                {
                    var data = from u in userRepository.FindBy(x => x.Status == true)
                               .OrderByDescending(x=>x.UpdationDate)
                                .Skip(StartIndex).Take(Limit)
                                group u by u.RoleId into g
                                select new {
                                    RoleId = g.Key,
                                    RoleName = g.FirstOrDefault().Role.RoleName,
                                    Users = g.Select(x => new {
                                        UserId =x.Id,
                                        x.UserName,
                                        x.ProfilePic,
                                        x.EmailID
                                    })                                    
                                };

                    var count = userRepository.FindBy(x => x.Status == true).Count();
                    
                    return Ok(new { result = data, count = data.Count() });
                }

                else if (UserId != 0 && RoleId != 0)
                {
                    var data = from u in userRepository.FindBy(x =>x.Id == UserId && x.RoleId == RoleId && x.Status == true)
                               .OrderByDescending(x => x.UpdationDate)
                               .Skip(StartIndex).Take(Limit)
                               group u by u.RoleId into g
                               select new
                               {
                                   RoleId = g.Key,
                                   RoleName = g.FirstOrDefault().Role.RoleName,
                                   Users = g.Select(x => new {
                                       UserId = x.Id,
                                       x.UserName,
                                       x.ProfilePic,
                                       x.EmailID
                                   })
                               };

                    var count = userRepository.FindBy(x => x.Id == UserId && x.RoleId == RoleId && x.Status == true).Count();

                    return Ok(new { result = data, count = data.Count() });
                }
                else if (UserId == 0 && RoleId != 0)
                {
                    var data = from u in userRepository.FindBy(x => x.RoleId == RoleId && x.Status == true)
                               .OrderByDescending(x => x.UpdationDate)
                               .Skip(StartIndex).Take(Limit)
                               group u by u.RoleId into g
                               select new
                               {
                                   RoleId = g.Key,
                                   RoleName = g.FirstOrDefault().Role.RoleName,
                                   Users = g.Select(x => new {
                                       UserId = x.Id,
                                       x.UserName,
                                       x.ProfilePic,
                                       x.EmailID
                                   })
                               };

                    var count = userRepository.FindBy(x => x.RoleId == RoleId && x.Status == true).Count();

                    return Ok(new { result = data, count = data.Count() });                    
                }
                else if (UserId != 0 && RoleId == 0)
                {
                    var data = from u in userRepository.FindBy(x => x.Id == UserId && x.Status == true)
                               .OrderByDescending(x => x.UpdationDate)
                               .Skip(StartIndex).Take(Limit)
                               group u by u.RoleId into g
                               select new
                               {
                                   RoleId = g.Key,
                                   RoleName = g.FirstOrDefault().Role.RoleName,
                                   Users = g.Select(x => new {
                                       UserId = x.Id,
                                       x.UserName,
                                       x.ProfilePic,
                                       x.EmailID
                                   })
                               };

                    var count = userRepository.FindBy(x => x.Id == UserId && x.Status == true).Count();

                    return Ok(new { result = data, count = data.Count() });                    
                }

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpGet]
        //public IHttpActionResult GetUserPermission(long UserId, long RoleId)
        //{
        //    try
        //    {
        //        var headerList = (from frm in formManagementRepository.FindBy(x => x.FormParentId == null && x.Status == true)
        //                          select new { frm.Id, frm.FormName }).ToList();

        //        var data = from ur in userRoleRepository.FindBy(x => x.UserId == UserId && x.RoleId == RoleId && x.Status == true)
        //                   join rp in rolePermissionRepository.FindBy(x => x.Status == true)
        //                   on ur.RoleId equals rp.RoleId
        //                   join up in userPermissionRepository.FindBy(x => x.UserId == UserId && x.Status == true)
        //                   on rp.FormId equals up.FormId
        //                   into newDetails
        //                   from b in newDetails.DefaultIfEmpty(new UserPermission())
        //                   select new
        //                   {
        //                       Header = headerList.Where(x => x.Id == rp.FormManagement.FormParentId).FirstOrDefault().FormName,
        //                       rp.FormId,
        //                       rp.FormManagement.FormName,
        //                       Permission = JObject.Parse(b.Permission == null ? rp.Permission : b.Permission)
        //                   } into newList
        //                   group newList by newList.Header into g
        //                   select new
        //                   {
        //                       Header = g.Key,
        //                       Forms = g.Select(x => new { x.FormId, x.FormName, x.Permission })
        //                   };


        //        return Ok(new { result = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[HttpGet]
        //public IHttpActionResult GetUserPermission1(long UserId, long RoleId)
        //{
        //    try
        //    {
        //        var data = from pfrm in formManagementRepository.FindBy(x => x.Status == true && x.FormParentId == null).OrderBy(x => x.FormOrder)
        //                   join frm in formManagementRepository.FindBy(x => x.Status == true).OrderBy(x => x.FormOrder)
        //                   on pfrm.Id equals frm.FormParentId
        //                   join rp in rolePermissionRepository.FindBy(x => x.Status == true && x.RoleId == RoleId)
        //                   on frm.Id equals rp.FormId
        //                   into Details
        //                   from a in Details.DefaultIfEmpty(new RolePermission())
        //                   join up in userPermissionRepository.FindBy(x => x.UserId == UserId && x.Status == true)
        //                   on frm.Id equals up.FormId
        //                   into newDetails
        //                   from b in newDetails.DefaultIfEmpty(new UserPermission())
        //                   select new
        //                   {
        //                       Header = pfrm.FormName,
        //                       frm.Id,
        //                       frm.FormName,
        //                       Permission = b.Permission == null ? a.Permission : b.Permission
        //                   } into newList
        //                   group newList by newList.Header into g
        //                   select new
        //                   {
        //                       Header = g.Key,
        //                       Forms = g.Select(x => new { x.Id, x.FormName, Permission = JObject.Parse(x.Permission == null ? "{\"Add\":false,\"Edit\":false,\"Read\":false,\"Print\":false,\"Delete\":false}" : x.Permission) })
        //                   };

                
        //        return Ok(new { result = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }            
        //}
       
        [HttpGet]
        public IHttpActionResult GetRole()
        {
            return Ok(new { result = roleRepository.FindBy(x => x.Status == true) });
        }

        //[HttpGet]
        //public IHttpActionResult GetRolePermission()
        //{
        //    var data = from r in roleRepository.FindBy(x=>x.Status==true)
        //                  join rp in rolePermissionRepository.FindBy(x=>x.Status==true)
        //                  on r.Id equals rp.RoleId
        //                  join fm in formManagementRepository.FindBy(x=>x.Status==true)
        //                  on rp.FormId equals fm.Id
        //                  group rp by r.RoleName into g
        //                  select new
        //                  {
        //                      g.FirstOrDefault().Role.Id,
        //                      g.FirstOrDefault().Role.RoleName,
        //                      g.FirstOrDefault().Role.RoleDescription,
        //                      Forms=g.Select(x=>new {x.FormManagement.Id,x.FormManagement.FormName, Permission = JObject.Parse(x.Permission) })                                                                              
        //                  };
        //    return Ok(new { result= data });
        //}

        //[HttpGet]
        //public IHttpActionResult GetUserRole()
        //{
        //    var data = from ur in userRoleRepository.FindBy(x => x.Status == true)
        //               join u in userRepository.FindBy(x => x.Status == true)
        //               on ur.UserId equals u.Id
        //               join r in roleRepository.GetAll()
        //               on ur.RoleId equals r.Id
        //               select new
        //               {
        //                   Id = ur.Id,
        //                   UserName = u.UserName,
        //                   UserId=ur.UserId,
        //                   RoleId = ur.RoleId,
        //                   RoleName = r.RoleName                           
        //               };
        //    return Ok(new { result = data });
        //}

        //[HttpGet]
        //public IHttpActionResult SearchUserRole(string UserName,int limit)
        //{ 
        //    if(UserName== "undefined"|| UserName == "null" || UserName == null)
        //    {
        //        var data = (from ur in userRoleRepository.FindBy(x => x.Status == true)
        //                   select new
        //                   {
        //                       ur.UserId,
        //                       ur.User.UserName,                               
        //                       RoleId = ur.RoleId,
        //                       RoleName = ur.Role.RoleName,
        //                       ur.User.UserImage,
        //                       ur.User.UserEmail
        //                   }).Take(limit);

        //        return Ok(new { result = data });
        //    }
        //    else
        //    {
        //        var data = (from ur in userRoleRepository.FindBy(x => x.Status == true && x.User.UserName.Contains(UserName))                           
        //                   select new
        //                   {
        //                       ur.UserId,
        //                       ur.User.UserName,
        //                       RoleId = ur.RoleId,
        //                       RoleName = ur.Role.RoleName,
        //                       ur.User.UserImage,
        //                       ur.User.UserEmail
        //                   }).Take(limit);

        //        return Ok(new { result = data });
        //    }            
        //}

       
        
        [HttpGet]
        public IHttpActionResult GetFormGroupWise()
        {

            var data = from frm in formManagementRepository.FindBy(x => x.Status == true && x.FormParentId == null)
                       join fm in formManagementRepository.FindBy(x => x.Status == true && x.FormName.ToLower() != "divider")
                       on frm.Id equals fm.FormParentId
                       group fm by frm.FormName into g
                       select new
                       {
                           Header = g.Key,
                           Forms = g.Select(x => new { x.Id, x.FormName, Permission = new { Add = false, Edit = false, Read = false, Print = false, Delete = false } })
                       };
          //  var data = new { };
            return Ok(new { result = data });
        }

        private Nullable<bool> GetGroup(Nullable<int> ParentId)
        {
            Nullable<bool> val = null;
            if (ParentId == null)
                val = true;
            return val;
        }
        
        #endregion

        

   

      

      
        //[HttpPost]
        //public IHttpActionResult rolePermission(RolePermission model)
        //{
        //    if (model != null)
        //    {
        //        var _rolePermission = rolePermissionRepository.GetById(model.Id);

        //        if (_rolePermission == null)
        //        {
        //            if (rolePermissionRepository.GetByRoleId(model.RoleId) == null)
        //            {
        //                rolePermissionRepository.Create(new RolePermission()
        //                {
        //                    RoleId = model.RoleId,
        //                    Permission = model.Permission,
        //                    Status = true
        //                });
        //                return Ok(new { result = "Success", message = "Role Permission created Successfully!" });
        //            }
        //            else
        //                return Ok(new { result = "error", message = "Role Permission alredy Created !" });
        //        }
        //        else
        //        {
        //            _rolePermission.RoleId = model.RoleId;
        //            _rolePermission.Permission = model.Permission;                    
        //            rolePermissionRepository.Update(_rolePermission);
        //            return Ok(new { result = "Success", message = "Role Permission Successfully updated!" });
        //        }
        //    }
        //    return Ok(new { result = "error", message = "error" });           
        //}

        //[HttpPost]
        //public IHttpActionResult userRole(UserRoleViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (multiSelect _user in model.Users)
        //        {
        //            var _userRole = userRoleRepository.FindBy(x => x.UserId == _user.Id).FirstOrDefault();
        //            if(_userRole==null)
        //            {
        //                userRoleRepository.Create(new UserRole {
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
        public IHttpActionResult userPermission(UserPermissionViewModel models)
        {
            if(models!=null)
            {
                //Delete All permission of user
                if(models.Forms.Count!=0)
                {
                    //var _userPermission = userPermissionRepository.FindBy(x => x.UserId == models.UserId);

                    //foreach (var item in _userPermission)
                    //{
                    //    var _userPermission = userPermissionRepository.FindBy(x => x.UserId == models.UserId);
                    //    userPermissionRepository.Delete(item, false);
                    //}
                    //userPermissionRepository.Save();

                    foreach (var item in models.Forms)
                    {
                        var _UserPermission = userPermissionRepository.FindBy(x => x.UserId == models.UserId && x.FormId==item.FormId).FirstOrDefault();
                        if(_UserPermission==null)
                        {
                            userPermissionRepository.Create(new UserPermission
                            {
                                UserId = models.UserId,
                                FormId = item.FormId,
                                Add = item.Permission.Add,
                                Edit = item.Permission.Edit,
                                Delete = item.Permission.Delete,
                                Read = item.Permission.Read,
                                Print = item.Permission.Print,
                                Status = true
                            });
                        }
                        else
                        {                            
                            _UserPermission.Add = item.Permission.Add;
                            _UserPermission.Edit = item.Permission.Edit;
                            _UserPermission.Delete = item.Permission.Delete;
                            _UserPermission.Read = item.Permission.Read;
                            _UserPermission.Print = item.Permission.Print;

                            userPermissionRepository.Update(_UserPermission);
                        }
                        
                    }
                }                
                return Ok(new { result = "Success", message = "User Permission created Successfully!" });
            }
            return Ok(new { result = "error", message = "error" });
        }
       // #endregion

        #region Delete
        [HttpDelete]
        public IHttpActionResult DeleteUser(int Id)
        {
            if (Id != 0)
            {
                var _user = userRepository.GetById(Id);
                _user.Status = false;
                userRepository.Update(_user);
                return Ok(new { result = "Success", message = "Role deleted sucessfully" });
            }
            return Ok(new { result = "error", message = "" });
        }
        [HttpDelete]
        public IHttpActionResult DeleteRole(long Id)
        {
            if(Id!=0)
            {
                var _role = roleRepository.GetById(Id);
                _role.Status = false;
                roleRepository.Update(_role);
                DeleteRolePermission(Id);
                return Ok(new { result = "Success", message = "Role deleted sucessfully" });
            }
            return Ok(new { result = "error", message = "" });
        }

        private void DeleteRolePermission(long Id)
        {
            var rolePermissionList = rolePermissionRepository.FindBy(x => x.RoleId == Id);
            rolePermissionRepository.DeleteWhere(rolePermissionList);
            rolePermissionRepository.Save();
        }
        //[HttpDelete]
        //public IHttpActionResult DeleteRolePermission(int Id)
        //{
        //    if (Id != 0)
        //    {
        //        rolePermissionRepository.Delete(rolePermissionRepository.GetById(Id));
        //        return Ok(new { result = "Success", message = "Role Permission deleted sucessfully" });
        //    }
        //    return Ok(new { result = "error", message = "error" });
        //}

        //public IHttpActionResult DeleteUserRole(int Id)
        //{
        //    if (Id != 0)
        //    {                              
        //        userRoleRepository.Delete(userRoleRepository.GetById(Id));
        //        return Ok(new { result = "Success", message = "Role deleted sucessfully" });
        //    }
        //    return Ok(new { result = "error", message = "" });
        //}

        #endregion

       
    }
}
