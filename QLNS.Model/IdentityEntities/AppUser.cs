using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QLNS.Model.IdentityEntities
{
    public class AppUser : IdentityUser<long, AppLogin, AppUserRole, AppClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser,long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public override long Id { get; set; }
        public override string UserName { get; set; }
        public override string Email { get; set; }
        public override string PhoneNumber { get; set; }
        public DateTime? BirthDay { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        [StringLength(250)]
        public string FullName { get; set; }
        public string Avatar { get; set; }
        
    }
    public class AppUserRole : IdentityUserRole<long>
    {

    }

    public class AppRole : IdentityRole<long, AppUserRole>
    {

    }

    public class AppClaim : IdentityUserClaim<long>
    {

    }
    public class AppLogin : IdentityUserLogin<long>
    {

    }
}
