using HCM.Data.Models;
using HCM.Web.ViewModels.User;

namespace HCM.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<ProfileViewModel> GetProfileInfoAsync(string id);
        Task<ICollection<ApplicationUser>> GetAllByDepartmentAsync(string id);
        Task<ApplicationUser> GetByEmployeeIdAsync(string id);
        Task<bool> IsManagingEmployeeAsync(string userId, string employeeId);
    }
}
