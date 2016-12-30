using listening.Models;
using listening.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listening.Data
{
    public static class SampleData
    {
        private static ApplicationDbContext _context;
        private static UserManager<ApplicationUser> _userManager;

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            _context = (ApplicationDbContext)serviceProvider.GetService(typeof(ApplicationDbContext));
            _userManager = (UserManager<ApplicationUser>)serviceProvider.GetRequiredService(
                typeof(UserManager<ApplicationUser>));

            CreateRoles();

            foreach (var user in SecurityRulesSingleton.Instance.Rules.Users)
            {
                var appUser = new ApplicationUser { UserName = user.UserName, Email = user.Email };
                await CreateUsers(appUser, user.Role, user.Password);
            }

            _context.SaveChanges();
        }

        private static void CreateRoles()
        {
            var roles = SecurityRulesSingleton.Instance.Rules.Users.Select(x => x.Role).Distinct();
            var identityRoles =
                    roles.Select(x => new IdentityRole { Name = CapitalizeFirstLetter(x), NormalizedName = x.ToUpper() })
                        .ToArray();

            foreach (var role in identityRoles)
                if (!_context.Roles.Any(x => x.Name.Equals(role.Name)))
                    _context.Roles.Add(role);
        }

        private static async Task CreateUsers(ApplicationUser user, string role, string password)
        {
            var existingAdminUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingAdminUser != null)
            {
                if (!(await _userManager.IsInRoleAsync(existingAdminUser, role)))
                    await _userManager.AddToRoleAsync(existingAdminUser, role);
            }
            else
            {
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        private static string CapitalizeFirstLetter(string str)
        {
            var first = str.First().ToString().ToUpper();
            var last = str.Substring(1, str.Length - 1);
            return $"{first}{last}";
        }
    }
}
