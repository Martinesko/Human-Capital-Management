

namespace HCM.Common
{
    public static class HCMConstants
    {
        public static class DepartmentConstants
        {
            public const string Engineering = "11111111-1111-1111-1111-111111111111";
            public const string Finance = "22222222-2222-2222-2222-222222222222";
            public const string IT = "33333333-3333-3333-3333-333333333333";
        }
        
        public static class EmployeeConstants
        {
            public const string AliceId = "44444444-4444-4444-4444-444444444444";
            public const string BobId = "55555555-5555-5555-5555-555555555555";
            public const string CarolId = "66666666-6666-6666-6666-666666666666";
        }

        public static class RoleConstants
        {
            public const string EmployeeRoleName = "Employee";
            public const string ManagerRoleName = "Manager";
            public const string HRAdminRoleName = "HRAdmin";
        }

        public static class LoginConstants
        {
            public const string HRAdminEmail = "hradmin@example.com";
            public const string HRAdminPassword = "AdminPassword123!";

            public const string ManagerEmail = "bob.smith@example.com";
            public const string ManagerPassword = "ManagerPassword123!";

            public const string AliceEmail = "alice.johnson@example.com";
            public const string AlicePassword = "AlicePassword123!";

            public const string CarolEmail = "carol.williams@example.com";
            public const string CarolPassword = "CarolPassword123!";
        }
    }
}
