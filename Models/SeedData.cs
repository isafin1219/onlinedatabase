using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineDatabase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineDatabase.Models
{
    public class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();

            string AdminUserName = "Admin";
            
            var AdminUser = await userManager.FindByNameAsync(AdminUserName);
            if (AdminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "isafin1219@gmail.com",
                    Email = "isafin1219@gmail.com",
                    Name = "Ilnur",
                    Surname = "Safin",
                    JobTitle = "Administrator",
                    Stations = "GYD",
                    PhoneNumber = "+994502531012"
                };               

                var result = await userManager.CreateAsync(user, "gu48u6y");
                if (result.Succeeded)
                {
                    var AdminRole = await roleManager.FindByNameAsync("Admin");

                    if (AdminRole == null)
                    {
                        // first we create Admin rool   
                        var role = new IdentityRole
                        {
                            Name = "Admin"
                        };
                        await roleManager.CreateAsync(role);
                        await roleManager.AddClaimAsync(AdminRole, new Claim("Manage Users", "manage.users"));
                        await userManager.AddClaimAsync(user, new Claim("Manage Users", "manage.users"));

                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await roleManager.AddClaimAsync(AdminRole, new Claim("Manage Users", "manage.users"));
                        await userManager.AddClaimAsync(user, new Claim("Manage Users", "manage.users"));
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
            var OperationsRole = await roleManager.FindByNameAsync("Operations");
            if (OperationsRole == null)
            {
                // first we create Admin rool   
                var role = new IdentityRole
                {
                    Name = "Operations"
                };
                await roleManager.CreateAsync(role);                
            }
            var StationRole = await roleManager.FindByNameAsync("Station");
            if (StationRole == null)
            {
                // first we create Admin rool   
                var role = new IdentityRole
                {
                    Name = "Station"
                };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
