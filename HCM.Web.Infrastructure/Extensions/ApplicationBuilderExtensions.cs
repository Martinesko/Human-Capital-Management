using HCM.Data;
using HCM.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static HCM.Common.HCMConstants.RoleConstants;
using static HCM.Common.SeedConstants.LoginConstants;
using static HCM.Common.SeedConstants.EmployeeConstants;
using static HCM.Common.SeedConstants.DepartmentConstants;

namespace HCM.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedRoles(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var roles = new[] { EmployeeRoleName, ManagerRoleName, HRAdminRoleName };

                Task.Run(async () =>
                {
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new ApplicationRole { Name = role });
                        }
                    }
                }).GetAwaiter().GetResult();
            }
            return app;
        }

        public static IApplicationBuilder SeedHRAdminUser(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                if (!roleManager.RoleExistsAsync(HRAdminRoleName).Result)
                {
                    roleManager.CreateAsync(new ApplicationRole { Name = HRAdminRoleName }).Wait();
                }

                var user = userManager.FindByEmailAsync(HRAdminEmail).Result;
                if (user == null)
                {
                    var hrAdmin = new ApplicationUser
                    {
                        UserName = HRAdminEmail,
                        Email = HRAdminEmail,
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(hrAdmin, HRAdminPassword).Wait();
                    userManager.AddToRoleAsync(hrAdmin, HRAdminRoleName).Wait();
                }
            }
            return app;
        }

        public static IApplicationBuilder SeedManagerUserAndDepartmentManager(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<HcmDbContext>();

                var managerEmail = ManagerEmail;
                var managerPassword = ManagerPassword;
                var managerUser = userManager.FindByEmailAsync(managerEmail).Result;
                
                if (managerUser == null)
                {
                    var manager = new ApplicationUser
                    {
                        UserName = managerEmail,
                        Email = managerEmail,
                        EmailConfirmed = true,
                        EmployeeId = Guid.Parse(BobId) 
                    };
                    userManager.CreateAsync(manager, managerPassword).Wait();
                    userManager.AddToRoleAsync(manager, ManagerRoleName).Wait();
                }

                Guid departmentId = Guid.Parse(Finance); 
                bool exists = dbContext.DepartmentsManagers
                    .Any(dm => dm.ManagerId == Guid.Parse(BobId) && dm.DepartmentId == departmentId);
                if (!exists)
                {
                    dbContext.DepartmentsManagers.Add(new DepartmentManager
                    {
                        ManagerId = Guid.Parse(BobId),
                        DepartmentId = departmentId
                    });
                    dbContext.SaveChanges();
                }
            }
            return app;
        }

        public static IApplicationBuilder SeedEmployeeUsers(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var aliceEmail = AliceEmail;
                var alicePassword = AlicePassword;

                var aliceUser = userManager.FindByEmailAsync(aliceEmail).Result;
                if (aliceUser == null)
                {
                    var alice = new ApplicationUser
                    {
                        UserName = aliceEmail,
                        Email = aliceEmail,
                        EmailConfirmed = true,
                        EmployeeId = Guid.Parse(AliceId)
                    };
                    userManager.CreateAsync(alice, alicePassword).Wait();
                    userManager.AddToRoleAsync(alice, EmployeeRoleName).Wait();
                }

                var carolEmail = CarolEmail;
                var carolPassword = CarolPassword;
                var carolUser = userManager.FindByEmailAsync(carolEmail).Result;
                if (carolUser == null)
                {
                    var carol = new ApplicationUser
                    {
                        UserName = carolEmail,
                        Email = carolEmail,
                        EmailConfirmed = true,
                        EmployeeId = Guid.Parse(CarolId)
                    };
                    userManager.CreateAsync(carol, carolPassword).Wait();
                    userManager.AddToRoleAsync(carol, EmployeeRoleName).Wait();
                }
            }
            return app;
        }
    }
}
