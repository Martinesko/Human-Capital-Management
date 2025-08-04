using HCM.Data;
using HCM.Data.Models;
using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.Department;
using Microsoft.EntityFrameworkCore;

namespace HCM.Services.Data
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HcmDbContext context;

        public DepartmentService(HcmDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<DepartmentViewModel>> GetAllAsync()
        {
            return await context.Departments
                .AsNoTracking()
                .Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();
        }

        public async Task CreateAsync(DepartmentFormModel model)
        {
            var department = new Department
            {
                Name = model.Name,
            };

            await context.AddAsync(department);
            context.SaveChanges();
        }

        public async Task DeleteAsync(string id)
        {
            var department = await context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Managers)
                .FirstAsync(d => d.Id == Guid.Parse(id));

            context.RemoveRange(department.Managers);
            context.RemoveRange(department.Employees);
            context.Remove(department!);
            await context.SaveChangesAsync();
        }

        public async Task<DepartmentFormModel> GetByIdAsync(string id)
        {
            return await context.Departments.Where(d => d.Id == Guid.Parse(id))
                .AsNoTracking()
                .Select(u => new DepartmentFormModel()
                {
                    Id = u.Id.ToString(),
                    Name = u.Name
                }).FirstAsync();
        }

        public async Task EditAsync(DepartmentFormModel model)
        {
            var department = await context.Departments.FirstAsync(d => d.Id == Guid.Parse(model.Id));

            department.Name = model.Name;

            context.Update(department);
            await context.SaveChangesAsync();
        }
    }
}
