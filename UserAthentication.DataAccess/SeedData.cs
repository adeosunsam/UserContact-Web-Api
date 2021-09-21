using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAthentication.Common;
using UserAthentication.Model;

namespace UserAthentication.DataAccess
{
    public class SeedData
    {
        public async static Task Seeder(RoleManager<IdentityRole> roleManager, UserManager<Users> userManager, ContactAppContext context)
        {
            try
            {
                 await context.Database.EnsureCreatedAsync();
                if (!context.Users.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = UserRole.Admin.ToString() });
                    await roleManager.CreateAsync(new IdentityRole { Name = UserRole.Customer.ToString() });

                    var userList = new List<Users>
                    {
                        new Users
                        {
                            FirstName = "Samuel",
                            LastName = "Adeosun",
                            Email = "samuel@gmail.com",
                            UserName = "Allos",
                            PhoneNumber = "08165434179",
                            PasswordHash = "Samuel1234@"
                        },
                        new Users
                        {
                            FirstName = "Gideon",
                            LastName = "Faive",
                            Email = "gideon@gmail.com",
                            UserName = "faive",
                            PhoneNumber = "08143547856",
                            PasswordHash = "Gideon1234@"
                        },
                        new Users
                        {
                            FirstName = "Ombu",
                            LastName = "Ayebakuro",
                            Email = "kuro@gmail.com",
                            UserName = "iceboss",
                            PhoneNumber = "08186957401",
                            PasswordHash = "Kuro1234@"
                        }
                    };

                    foreach (var user in userList)
                    {
                        await userManager.CreateAsync(user, user.PasswordHash);
                        if (user == userList[0])
                        {
                            await userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
                        }
                        else
                            await userManager.AddToRoleAsync(user, UserRole.Customer.ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
