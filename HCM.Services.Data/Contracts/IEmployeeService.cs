using HCM.Web.ViewModels.Employee;

namespace HCM.Services.Data.Contracts
{
    public interface IEmployeeService
    {
        EmployeeInfoViewModel? GetEmployeeInfoByUserId(string userId);
    }
}
