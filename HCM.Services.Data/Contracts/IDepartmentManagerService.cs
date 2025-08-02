using HCM.Web.ViewModels.DepartmentManagerService;
using HCM.Web.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCM.Services.Data.Contracts
{
    public interface IDepartmentManagerService
    {
        Task CreateAsync(DepartmentManagerViewModel model);

        Task DeleteAsync(string id);
    }
}
