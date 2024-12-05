using LibrarySystem.Core.Entitties.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Repository.Data.Contexts
{
    public static class IdentityContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new AppRole { Name = "Admin" });

            var adminUser = await userManager.FindByEmailAsync("admin@library.com");
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin@library.com",
                    DisplayName = "LibraryAdmin",
                    Email = "admin@library.com",
                    PhoneNumber = "01013818970"
                };
                var result = await userManager.CreateAsync(adminUser, "adminPa$$w0rd");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                else
                    foreach (var error in result.Errors)
                        Console.WriteLine($"Error: {error.Description}");
            }
        }
            public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
            {
                var roles = new[] { "Admin", "Librarian", "User" }; // الأدوار المطلوبة

                foreach (var role in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(role);
                    if (!roleExist)
                    {
                        // إذا كان الدور غير موجود، قم بإنشائه
                        await roleManager.CreateAsync(new AppRole { Name = role });
                    }
                }
            }

    }

}