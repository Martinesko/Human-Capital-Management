using HCM.Data;
using HCM.Services.Data.Contracts;
using HCM.Web.ViewModels.Department;
using HCM.Web.ViewModels.Role;
using Microsoft.EntityFrameworkCore;

namespace HCM.Services.Data
{
    public class RoleService : IRoleService
    {
        private readonly HcmDbContext context;

        public RoleService(HcmDbContext context)
        {
            this.context = context;
        }
        public async Task<ICollection<RoleViewModel>> AllAsync()
        {
            return await context.Roles
             .Select(r => new RoleViewModel
             {
                 Id = r.Id,
                 Name = r.Name!
             })
             .ToListAsync();
        }
    }
}
