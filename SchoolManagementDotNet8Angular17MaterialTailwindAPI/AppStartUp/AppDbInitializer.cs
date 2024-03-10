using Microsoft.AspNetCore.Identity;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Consts;

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
            }
        }
    }
}
