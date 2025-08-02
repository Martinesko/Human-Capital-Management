using HCM.Data;
using HCM.Data.Models;
using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.DepartmentManagerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HCM.Services.Data
{
    public class DepartmentManagerService : IDepartmentManagerService
    {
        private readonly HcmDbContext context;

        public DepartmentManagerService(HcmDbContext context)
        {
            this.context = context;
        }
        public async Task CreateAsync(DepartmentManagerViewModel model)
        {
            if (context.Departments.Any(d => d.Id == Guid.Parse(model.DepartmentId)) && context.Employees.Any(e => e.Id == Guid.Parse(model.ManagerId)))
            {
                await context.DepartmentsManagers.AddAsync(new DepartmentManager
                {
                    DepartmentId = Guid.Parse(model.DepartmentId),
                    ManagerId = Guid.Parse(model.ManagerId)
                });
                await context.SaveChangesAsync();

            }
            Console.WriteLine("No working");
        }

        public async Task DeleteAsync(string id)
        {
            var managerId = Guid.Parse(id);

            var departmentManagerEntries = await context.DepartmentsManagers
                .Where(dm => dm.ManagerId == managerId)
                .ToListAsync();

            if (departmentManagerEntries.Any())
            {
                context.DepartmentsManagers.RemoveRange(departmentManagerEntries);
                await context.SaveChangesAsync();
            }
        }
    }
}
