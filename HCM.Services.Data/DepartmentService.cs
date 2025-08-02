using HCM.Data;
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
        public async Task<ICollection<DepartmentViewModel>> AllAsync()
        {
            return await context.Departments
                .Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();
        }
    }
}
