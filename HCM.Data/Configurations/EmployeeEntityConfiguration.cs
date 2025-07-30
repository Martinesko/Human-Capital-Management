using HCM.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static HCM.Common.HCMConstants.DepartmentConstants;
using static HCM.Common.HCMConstants.EmployeeConstants;

namespace HCM.Data.Configurations
{
    class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData(new Employee[]
            {
                new Employee
                {
                    Id = Guid.Parse(AliceId),
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@example.com",
                    JobTitle = "HR Manager",
                    Salary = 60000m,
                    DepartmentId = Guid.Parse(HumanResources)
                },
                new Employee
                {
                    Id = Guid.Parse(BobId),
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "bob.smith@example.com",
                    JobTitle = "Accountant",
                    Salary = 55000m,
                    DepartmentId = Guid.Parse(Finance)
                },
                new Employee
                {
                    Id = Guid.Parse(CarolId),
                    FirstName = "Carol",
                    LastName = "Williams",
                    Email = "carol.williams@example.com",
                    JobTitle = "IT Specialist",
                    Salary = 65000m,
                    DepartmentId = Guid.Parse(IT)
                }
            });
        }
    }
}

