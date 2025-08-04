using System;
using HCM.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static HCM.Common.SeedConstants.DepartmentConstants;

namespace HCM.Data.Configurations
{
    internal class DepartmentEntityConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(
            [
                new Department
                {
                    Id = Guid.Parse(Engineering),
                    Name = "Engineering"
                },
                new Department
                {
                    Id = Guid.Parse(Finance),
                    Name = "Finance"
                },
                new Department
                {
                    Id = Guid.Parse(IT),
                    Name = "IT"
                }
            ]);
        }
    }
}
