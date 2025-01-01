using Microsoft.AspNetCore.Identity;

namespace boilerplate_app.Infrastructure.Data
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "User", "Manager", "Admin" };

            foreach (var role in roles)
            {
                if (!await RoleManager.ExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
