using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace listening.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser { }

    //public class UserRole : IdentityUserRole<long> { }
    //public class RoleClaim : IdentityRoleClaim<long> { }
    //public class Role : IdentityRole<long, UserRole, RoleClaim> {
    //    public Role() : base() { }
    //    public Role(string roleName) : base(roleName) { }
    //}
    //public class UserClaim : IdentityUserClaim<long> { }
    //public class UserLogin : IdentityUserLogin<long> { }
    //public class UserToken : IdentityUserToken<long> { }

    //public class ApplicationUser : IdentityUser<long, UserClaim, UserRole, UserLogin>
    //{
    //    public ApplicationUser() : base() { }
    //    public ApplicationUser(string userName) : base(userName) { }
    //}
}
