using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QLNS.Model.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.Seed
{
    public class InitAccountSeed
    {
        public static void  init(QLNS.Model.QLNSContext context)
        {
            var AccountAdminName = "admin";
            var AccountAdminPassword = "12345678";
            var userAdmin = context.Users.Where(x => x.UserName == AccountAdminName).FirstOrDefault();
            if (userAdmin == null)
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<AppUser, AppRole, long, AppLogin, AppUserRole, AppClaim>(context);
                var userManager = new UserManager<AppUser, long>(userStore);
                var user = new AppUser { UserName = AccountAdminName };
                userManager.Create(user, AccountAdminPassword);
            }
        }

    }
}
