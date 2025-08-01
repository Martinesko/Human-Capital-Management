using HCM.Data;
using HCM.Data.Models;
using HCM.Services.Data;
using HCM.Services.Data.Contracts;
using HCM.Web.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HCM.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
       
        builder.Services.AddDbContext<HcmDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
            options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
            options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
            options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
            options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigit");
        })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<HcmDbContext>();

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();


        builder.Services.AddScoped<IEmployeeService, EmployeeService>();

        var app = builder.Build();

        app.SeedRoles();
        app.SeedHRAdminUser();
        app.SeedEmployeeUsers();
        app.SeedManagerUserAndDepartmentManager();

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.Equals("/Identity/Account/Register", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Redirect("/Identity/Account/Login");
                return;
            }
            await next();
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
