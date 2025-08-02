using X.PagedList;
using static HCM.Common.HCMConstants.GeneralConstants;

namespace HCM.Web.ViewModels.Employee
{
    public class EmployeeAllViewModel
    {
        public EmployeeAllViewModel()
        {
            CurrentPage = DefaultPage;
        }

        public int CurrentPage { get; set; }

        public IPagedList<EmployeeViewModel> Employees { get; set; } = null!;
    }
}
