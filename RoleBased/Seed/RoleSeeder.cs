using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RoleBased.Models;
using RoleBased.Seed;

namespace RoleProductMvc.Seed
{
    public class RoleSeeder : IRoleSeeder
    {
        private readonly IServiceProvider _provider;
        public RoleSeeder(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task SeedRolesAndUsersAsync()
        {
            using var scope = _provider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = new[] { "Admin", "Manager" };

            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                    await roleManager.CreateAsync(new IdentityRole(r));
            }

            
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = "admin", Email = "admin@example.com", EmailConfirmed = true };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            
            var managerUser = await userManager.FindByNameAsync("manager1");
            if (managerUser == null)
            {
                managerUser = new ApplicationUser { UserName = "manager1", Email = "manager1@example.com", EmailConfirmed = true };
                var result = await userManager.CreateAsync(managerUser, "Manager@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(managerUser, "Manager");
            }
        }
    }
}
