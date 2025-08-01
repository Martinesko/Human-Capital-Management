using HCM.Web.ViewModels.Employee;
using HCM.Web.ViewModels.Department;    
using Microsoft.AspNetCore.Mvc;

namespace HCM.Services.Data.Contracts
{
    public interface IEmployeeService
    {
        EmployeeInfoViewModel? GetEmployeeInfoByUserId(string userId);
        Task<List<EmployeeUserViewModel>> GetAllEmployeesAync();
        Task<bool> Create(CreateEmployeeViewModel model);
        Task<List<DepartmentViewModel>> GetAllDepartmentsAsync();
        Task DeleteEmployeeAndUserAsync(string userId);
        Task<CreateEmployeeViewModel> GetEmployeeForEditAsync(string userId);
        Task UpdateEmployeeAsync(CreateEmployeeViewModel model);

    }
}
