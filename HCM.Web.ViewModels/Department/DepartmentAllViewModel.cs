using static HCM.Common.HCMConstants.GeneralConstants;
using X.PagedList;

namespace HCM.Web.ViewModels.Department
{
    public class DepartmentAllViewModel
    {
        public DepartmentAllViewModel()
        {
            CurrentPage = DefaultPage;
        }

        public int CurrentPage { get; set; }
        public IPagedList<DepartmentViewModel> Departments { get; set; } = null!;
    }
}
