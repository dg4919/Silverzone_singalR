using SilverzoneERP.Entities.ViewModel.Admin;
using SilverzoneERP.Data;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Admin
{
    public class UserController : ApiController
    {
        private IAccountRepository accountRepository;
        private IRoleRepository roleRepository;

        [HttpPost]
        public IHttpActionResult Register_user(userViewModel model)
        {
            string _result = string.Empty;

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            var ipAddress = host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            if (ModelState.IsValid && roleRepository.role_isActive(model.RoleId))
            {
                using (var transaction = accountRepository.BeginTransaction())
                {
                    string password_hash = accountRepository.GetPasswordHash(model.Password);
                    foreach (string user in model.userName)
                    {
                        if (accountRepository.check_User(user, model.RoleId))
                        {
                            transaction.Rollback();
                            return Ok(new { result = "exist", msg = string.Format("{0} already exist, Data is not saved, try Again !", user) });
                        }

                        accountRepository.Create(new Entities.Models.User
                        {
                            EmailID = user,
                            Password = password_hash,
                            Browser = HttpContext.Current.Request.Browser.Browser,
                            IPAddress = ipAddress.ToString(),
                            OperatingSystem = Environment.OSVersion.ToString(),
                            Location = RegionInfo.CurrentRegion.DisplayName,
                            CreationDate = accountRepository.get_DateTime(),
                            UpdationDate = accountRepository.get_DateTime(),
                            RoleId = model.RoleId
                        }, false);
                    }
                    accountRepository.Save();
                    transaction.Commit();

                    return Ok(new { result = "ok" });
                }
            }
            return Ok(new { result = "invalid_Role" });
        }


        public UserController(
         IAccountRepository _accountRepository,
        IRoleRepository _roleRepository
        )
        {
            accountRepository = _accountRepository;
            roleRepository = _roleRepository;
        }

    }
}
