using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIddictServer.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }

        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var adminUser = new ApplicationUser()
            {
                Email = "littlepeterr@hotmail.com",
                UserName = "littlepeterr@hotmail.com",
                EmailConfirmed = true,
            };

            if (userManager.Users.All(u => u.Id != adminUser.Id))
            {
                var user = await userManager.FindByEmailAsync(adminUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(adminUser, "1234qW,");
                    await userManager.AddToRoleAsync(adminUser, Roles.Administrator.ToString());
                    await userManager.AddToRoleAsync(adminUser, Roles.Basic.ToString());
                }
            }
        }
    }
}
