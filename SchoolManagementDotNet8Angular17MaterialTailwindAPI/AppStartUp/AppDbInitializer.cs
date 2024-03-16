using Microsoft.AspNetCore.Identity;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Consts;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Entities;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.AppStartUp
{
    public static class AppDbInitializer
    {
        public static async Task SeedDB(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(StaticUserRoles.ADMIN))
                    await roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));

                if (!await roleManager.RoleExistsAsync(StaticUserRoles.USER))
                    await roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (await userManager.FindByEmailAsync("admin@admin.com") is null)
                {
                    ApplicationUser adminUser = new ApplicationUser()
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Email = "admin@admin.com",
                        UserName = "Admin",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(adminUser, "Admin1!");

                    await userManager.AddToRoleAsync(adminUser, StaticUserRoles.ADMIN);
                }

                if (await userManager.FindByEmailAsync("user@user.com") is null)
                {
                    ApplicationUser regularUser = new ApplicationUser()
                    {
                        FirstName = "User",
                        LastName = "User",
                        Email = "user@user.com",
                        UserName = "User",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(regularUser, "Admin1!");

                    await userManager.AddToRoleAsync(regularUser, StaticUserRoles.USER);
                }
            }
        }
    }
}
