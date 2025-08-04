using HCM.Web.ViewModels.Employee;

namespace HCM.Services.Data.Contracts
{
    public interface IEmployeeService
    {
        Task<ICollection<EmployeeViewModel>> GetAllAsync(EmployeeAllViewModel model, string id, bool isAdmin);
        Task<EmployeeCreateFormModel> GetCreateAsync(string id);
        Task<EmployeeEditFormModel> GetEditAsync(string id);
        Task DeleteAsync(string userId);
    }
}
