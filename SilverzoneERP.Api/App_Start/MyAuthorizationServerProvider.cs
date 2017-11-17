using Microsoft.Owin.Security.OAuth;
using SilverzoneERP.Context;
using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SilverzoneERP.Api
{
    // Code for genrating Token after verify user from login()
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var verification_type = context.Parameters.Where(f => f.Key == "verificationMode")
                                                      .Select(f => f.Value)
                                                      .SingleOrDefault();
            if(verification_type != null)
            context.OwinContext.Set<string>("verificationMode", verification_type[0]);

            context.Validated();
            return Task.FromResult<object>(null);
        }

        // Microsoft.Owin..Host.SystemWeb > must Require :)
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var mode = context.OwinContext.Get<string>("verificationMode");

            dynamic _user;
            string role = string.Empty;

            if (mode != null)
            {
                using (IAccountRepository accountRepository = new AccountRepository(new SilverzoneERPContext()))
                {
                    var userName = context.UserName;
                    var password = context.Password;

                    var verificationMode = (verificationMode)Enum.Parse(typeof(verificationMode), mode);

                    _user = accountRepository.check_User(userName, verificationMode);
                    role = _user.Role.RoleName;   // get values using navigation property
                }
            }
            else
            {
                using (IUserRepository userRepository = new UserRepository(new SilverzoneERPContext()))
                {
                    var userName = context.UserName;
                    var password = context.Password;

                    _user = userRepository.GetByEmail(userName);
                    role = _user.Role.RoleName;   // get values using navigation property
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            identity.AddClaim(new Claim(ClaimTypes.Name, _user.Id.ToString()));

            context.Validated(identity);
            return Task.FromResult<object>(null);
        }

    }
}