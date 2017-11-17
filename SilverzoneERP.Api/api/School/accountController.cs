using SilverzoneERP.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;
using SilverzoneERP.Entities.Models;
using System.Net;
using System.Web;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net.Sockets;
using SilverzoneERP.Entities;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class accountController : ApiController
    {
        IUserRepository _userRepository;
        IFormManagementRepository _formManagementRepository;
        IRolePermissionRepository _rolePermissionRepository;
        IUserPermissionRepository _userPermissionRepository;
        IEventYearRepository _eventYearRepository;
        //*****************  Constructor********************************

        public accountController(IUserRepository _userRepository, IFormManagementRepository _formManagementRepository, IRolePermissionRepository _rolePermissionRepository, IUserPermissionRepository _userPermissionRepository, IEventYearRepository _eventYearRepository)
        {
            this._userRepository = _userRepository;
            this._formManagementRepository = _formManagementRepository;
            this._rolePermissionRepository = _rolePermissionRepository;
            this._userPermissionRepository = _userPermissionRepository;
            this._eventYearRepository = _eventYearRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ERPuser user = _userRepository.SignIn(model.EmailID, model.Password);
                if (user != null)
                {                    
                    return Ok(new {
                        result = "Success",
                        user = user, token = getToken(user),
                        menu = GetFormUrlByUserId(user.RoleId,user.Id),
                        Event= _eventYearRepository.Get(DateTime.Now.Year, true)
                    });
                    //return Ok(new { result = "Success", user = user, token = getToken(user)});
                }
                else
                    return Ok(new { result = "error", message = "Email-Id and password does not matched. !" });
            }
            return Ok(new { result = "error", message = "error" });
        }

        private AccessTokenViewModel getToken(ERPuser model)
        {
            var url = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority;
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url + "/token");
            myWebRequest.ContentType = "application/x-www-form-urlencoded";
            myWebRequest.Method = "POST";

            var _role = model.Role.RoleName;



            var request = string.Format("grant_type=password&UserName={0}&Password={1}",
                                                                         HttpUtility.UrlEncode(model.EmailID),
                                                                         HttpUtility.UrlEncode(model.Password)
                                                                         );

            byte[] bytes = Encoding.ASCII.GetBytes(request);
            myWebRequest.ContentLength = bytes.Length;
            using (Stream outputStream = myWebRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }

            using (WebResponse webResponse = myWebRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AccessTokenViewModel));

                //Get deserialized object from JSON stream
                AccessTokenViewModel token = (AccessTokenViewModel)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }

        [HttpPost]
        public IHttpActionResult registration(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                

                if (model.Id == 0)
                {
                    if (_userRepository.GetByEmail(model.EmailID) == null)
                    {
                        ERPuser _user = new ERPuser();
                        _user.EmailID = model.EmailID;
                        _user.Status = true;

                        SetValue(model, ref _user);

                        _userRepository.Create(_user);

                        return Ok(new { result = "Success", message = "Successfully user created!" });
                    }
                    else
                        return Ok(new { result = "error", message = "User alredy Created !" });
                }
                else
                {
                    var _user = _userRepository.GetById(model.Id);
                    if (_user != null)
                    {
                        string Preserver_ProfilePic = _user.ProfilePic;
                        SetValue(model, ref _user);                        
                        _userRepository.Update(_user);

                       _userRepository.DeleteImage(Preserver_ProfilePic);
                        return Ok(new { result = "Success", message = "Successfully user updated !" });
                    }
                    return Ok(new { result = "error", message = "Failled user updation !" });
                }
            }
            return Ok(new { result = "error", message = "error" });
        }

        private void SetValue(UserViewModel model,ref ERPuser _user)
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            var ctx = HttpContext.Current;

            _user.UserName = model.UserName;
            _user.ProfilePic = _userRepository.WriteImage(model.ProfilePic);
            _user.Password = model.Password;
            _user.MobileNumber = model.MobileNumber;
            _user.GenderType = (genderType)model.GenderType;
            _user.RoleId = model.RoleId;
            _user.DOB = model.DOB;
            _user.Browser = ctx.Request.Browser.Browser;
            _user.OperatingSystem = Environment.OSVersion.ToString();
            _user.IPAddress = ipAddress.ToString();
            _user.UserAddress = model.UserAddress;
            _user.Location = RegionInfo.CurrentRegion.DisplayName;
        }
        [HttpGet]
        public IHttpActionResult GetUser(int StartIndex,int Limit)
        {
            try
            {
                long Count;
                return Ok(new { result = _userRepository.Get(StartIndex,Limit,out Count),Count= Count });
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        [HttpGet]
        public IHttpActionResult GetAllUser()
        {
            try
            {               
                return Ok(new { result = _userRepository.Get()});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IHttpActionResult GetUserPermission(long UserId,long RoleId)
        {
            return Ok(new { result = GetFormUrlByUserId( RoleId, UserId) });
        }

        private dynamic GetFormUrlByUserId(long RoleId, long UserId)
        {
            try
            {
                var FormPermissionList = from rp in _rolePermissionRepository.FindBy(x => x.RoleId == RoleId && x.Status == true)
                                         join up in _userPermissionRepository.FindBy(x => x.UserId == UserId && x.Status == true)
                                         on rp.FormId equals up.FormId
                                         into Details
                                         from a in Details.DefaultIfEmpty()
                                         select new
                                         {
                                             rp.FormId,
                                             Permission = a == null ? new { rp.Add, rp.Edit, rp.Delete, rp.Read, rp.Print } : new { a.Add, a.Edit, a.Delete, a.Read, a.Print }
                                         };

                var FormList = FormPermissionList.Select(x => x.FormId).ToArray();

                var data = from frm in _formManagementRepository.FindBy(x => x.FormParentId == null && x.Status == true).OrderBy(x => x.FormOrder)
                           select new
                           {
                               FormId = frm.Id,
                               frm.FormName,
                               frm.FormUrl,
                               Active = false,
                               Forms = frm.ChildFormManagement.Where(x => FormList.Contains(x.Id) && x.FormName.ToLower() != "divider" && x.Status == true).OrderBy(x => x.FormOrder).Select(subFrm => new
                               {
                                   FormId = subFrm.Id,
                                   subFrm.FormName,
                                   subFrm.FormUrl,
                                   Permission = FormPermissionList.AsEnumerable().FirstOrDefault(x => x.FormId == subFrm.Id).Permission,
                                   Active = false,
                                   subForms = subFrm.ChildFormManagement.OrderBy(x => x.FormOrder).Select(y => new {
                                       FormId = y.Id,
                                       y.FormName,
                                       y.FormUrl,
                                       Active = false,
                                   })

                               })

                           };
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private JObject ParsePermission(string Permission)
        {
            var data = JObject.Parse(Permission);
            return data;
        }
        //private dynamic GetFormUrlByUserId(long UserId)
        //{
        //    try
        //    {
        //        var headerList = (from frm in _formManagementRepository.FindBy(x => x.FormParentId == null && x.Status == true)
        //                          select new { frm.Id, frm.FormName }).ToList();

        //        var data = (from ur in _userRoleRepository.FindBy(x => x.UserId == UserId && x.Status == true)
        //                    join rp in _rolePermissionRepository.FindBy(x => x.Status == true)
        //                    on ur.RoleId equals rp.RoleId

        //                    join up in _userPermissionRepository.FindBy(x => x.Status == true && x.UserId == UserId)
        //                    on rp.FormId equals up.FormId
        //                    into Details
        //                    from a in Details.DefaultIfEmpty(new UserPermission())

        //                    select new
        //                    {
        //                        header = headerList.Where(x => x.Id == rp.FormManagement.FormParentId).FirstOrDefault().FormName,
        //                        rp.FormManagement.Id,
        //                        rp.FormManagement.FormName,
        //                        rp.FormManagement.FormUrl,
        //                        rp.FormManagement.FormOrder,
        //                        Permission = JObject.Parse(a.Permission == null ? rp.Permission : a.Permission)
        //                    }
        //                   into newList
        //                    group newList by newList.header into g
        //                    select new
        //                    {
        //                        Header = g.Key,
        //                        Forms = g.Select(x => new { x.Id, x.FormName, x.FormUrl, Permission = x.Permission, x.FormOrder }).OrderBy(x => x.FormOrder),
        //                    }).ToList();

        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpPost]
        public IHttpActionResult Active_Deactive(List<multiSelect> model)
        {
            try
            {
                foreach (var item in model)
                {
                    var _user = _userRepository.Get(item.Id);
                    if (_user != null)
                    {
                        _user.Status = !_user.Status;
                        _userRepository.Update(_user);
                    }
                }

                return Ok(new { result = "Success", message = "Successfully Save Changed !" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
