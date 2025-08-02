using HCM.Web.ViewModels.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCM.Services.Data.Contracts
{
    public interface IDepartmentService
    {
        Task<ICollection<DepartmentViewModel>> AllAsync();
    }
}
