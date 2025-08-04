namespace HCM.Common
{
    public static class ValidaitonConstants
    {
        public static class Department
        {
            public const int NameMaxLength = 50;
        }
        public static class Employee
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int EmailMaxLength = 100;
            public const int JobTitleMaxLength = 100;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;

            public const string DepartmentDisplayName = "Department";
            public const string RoleDisplayName = "Role Name";
            public const string ConfirmPasswordDisplayName = "Confirm Password";
            public const string JobTitleDisplayName = "Job Title";
            public const string FirstNameDisplayName = "First Name";
            public const string LastNameDisplayName = "Last Name";

            public const string PasswordsDontMatch = "Passwords do not match.";
        }
    }
}
