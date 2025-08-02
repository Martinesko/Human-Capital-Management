using HCM.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using static HCM.Common.SeedConstants.DepartmentConstants;
using static HCM.Common.SeedConstants.EmployeeConstants;

namespace HCM.Data.Configurations
{
    internal class DepartmentManagerEntityConfiguration : IEntityTypeConfiguration<DepartmentManager>
    {
        public void Configure(EntityTypeBuilder<DepartmentManager> builder)
        {
            builder.HasData(
            [
                new DepartmentManager
                {
                    ManagerId = Guid.Parse(BobId),
                    DepartmentId = Guid.Parse(Finance)
                }
            ]);
        }
    }
}
