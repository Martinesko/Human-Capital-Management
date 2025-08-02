using HCM.Web.ViewModels.Employee;

namespace HCM.Services.Data.Contracts
{
    public interface IEmployeeService
    {
        Task<ProfileViewModel> GetByIdAsync(string userId);
        Task<ICollection<EmployeeViewModel>> AllAsync(EmployeeAllViewModel model, string id, bool isAdmin);
        Task<Guid> CreateAsync(EmployeeFormModel model);
        Task DeleteAsync(string userId);
        Task<EmployeeFormModel> GetEditAsync(string userId);
        Task UpdateAsync(EmployeeFormModel model);
    }
}
