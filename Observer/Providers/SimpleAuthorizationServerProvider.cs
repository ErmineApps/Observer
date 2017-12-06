using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using Observer.Data.Repositories;


namespace Observer.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            IdentityUser user = null;

            using (AuthRepository authRepository = new AuthRepository())
            {
                user = await authRepository.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }



                var claims = await authRepository.GetUserClaims(user.Id);
                var roles = await authRepository.GetUserRoles(user.Id);

                var userRole = roles.FirstOrDefault();

                if (userRole.Equals("jurist"))
                {
                    using (var db = new ObserverDBEntities())
                    {
                        var id = user.Claims.FirstOrDefault(v => v.ClaimType.Equals("accountId"))?.ClaimValue;
                        int accountId = Convert.ToInt32(id);

                        var jurist = await db.Users.FindAsync(accountId);
                    }
                }

                identity.AddClaims(claims);

                string stringRoles = "";

                foreach (var role in roles)
                {
                    stringRoles += $"{role},";
                }

                identity.AddClaim(new Claim("sub", context.UserName));
                identity.AddClaim(new Claim("role", stringRoles));
            }

            context.Validated(identity);
        }
    }
}