using HCM.Web.ViewModels.Department;

namespace HCM.Services.Data.Contracts
{
    public interface IDepartmentService
    {
        Task<DepartmentFormModel> GetByIdAsync(string id);
        Task<ICollection<DepartmentViewModel>> GetAllAsync();
        Task CreateAsync(DepartmentFormModel model);
        Task EditAsync(DepartmentFormModel model);
        Task DeleteAsync(string id);
    }
}
