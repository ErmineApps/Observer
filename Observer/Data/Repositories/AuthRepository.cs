using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Observer.Data.Repositories
{
    public class AuthRepository : IDisposable
    {
        private readonly AuthContext _ctx;

        public readonly UserManager<IdentityUser> UserManager;
        public readonly RoleManager<IdentityRole> RoleManager;

        public Dictionary<string, string> RolesDictionaty;

        string roleName = "Moderator";

        public AuthRepository()
        {
            _ctx = new AuthContext();
            UserManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
            RolesDictionaty = new Dictionary<string, string>
            {
                {"Client", "client"},  
                {"DutyJurist", "dutyjurist"},
                {"Moderator", "moderator"}
            };
        }

        public async Task<IdentityResult> RegisterUser(Users userModel)
        {
            if (userModel == null)
                return new IdentityResult(new List<string> { "Bad data" });

          
            return await CreateUser(userModel);
        }

        public async Task CreateBaseRoles()
        {

            foreach (var role in RolesDictionaty.Values)
            {
                var existRole = await RoleManager.FindByNameAsync(role);
                if (existRole == null) await RoleManager.CreateAsync(new IdentityRole()
                {
                    Name = role
                });
            }
        }

        public async Task<IdentityResult> CreateUser(Users userModel)
        {
            if (userModel == null)
                return new IdentityResult(new List<string> { "Bad data" });

            IdentityUser user = new IdentityUser
            {
                UserName = userModel.Name
            };

            var result = await UserManager.CreateAsync(user, userModel.Password);

            await UserManager.AddToRolesAsync(user.Id, RolesDictionaty[userModel.Name]);

            await UserManager.AddClaimAsync(user.Id, new Claim("accountId", userModel.Id.ToString()));

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await UserManager.FindAsync(userName, password);

            return user;
        }

        public async Task<bool> FindExistUser(string userName)
        {
            IdentityUser user = await UserManager.FindByNameAsync(userName);

            if (user != null) return true;

            return false;
        }

        public async Task<IdentityUser> GetUser(string userName)
        {
            IdentityUser user = await UserManager.FindByNameAsync(userName);

            return user;
        }


        public async Task<IdentityUser> FindUser(string userName)
        {
            return await UserManager.FindByNameAsync(userName);
        }

        public async Task<IEnumerable<Claim>> GetUserClaims(string id)
        {
            return await UserManager.GetClaimsAsync(id);
        }

        public async Task<IEnumerable<string>> GetUserRoles(string id)
        {
            return await UserManager.GetRolesAsync(id);
        }

        public IEnumerable<string> GetUserRolesByName(string name)
        {
            var user = UserManager.FindByName(name);
            return UserManager.GetRoles(user.Id);
        }

        public void Dispose()
        {
            _ctx.Dispose();
            UserManager.Dispose();
            RoleManager.Dispose();
        }
    }
}