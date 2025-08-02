using HCM.Web.ViewModels.Role;

namespace HCM.Services.Data.Contracts
{
    public interface IRoleService
    {
        Task<ICollection<RoleViewModel>> AllAsync();

    }
}
