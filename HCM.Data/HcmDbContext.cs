using HCM.Data.Configurations;
using HCM.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace HCM.Data
{
    public class HcmDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {

        bool seed;

        public HcmDbContext(DbContextOptions<HcmDbContext> options , bool seed = true) : base(options) 
        {
            if (!Database.IsRelational())
            {
                Database.EnsureCreated();
            }
            this.seed = seed;
        }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentManager> DepartmentsManagers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DepartmentManager>()
                .HasKey(m => new { m.ManagerId, m.DepartmentId });

            modelBuilder.Entity<DepartmentManager>()
                .HasOne(dm => dm.Manager)
                .WithMany(e => e.ManagedDepartments)
                .HasForeignKey(dm => dm.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.UsersRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<ApplicationRole>()
                .HasMany(e => e.UsersRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(e => e.Role)
                .WithMany(e => e.UsersRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(e => e.User)
                .WithMany(e => e.UsersRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();


            if (seed)
            {
                modelBuilder.ApplyConfiguration(new DepartmentEntityConfiguration());
                modelBuilder.ApplyConfiguration(new EmployeeEntityConfiguration());
                modelBuilder.ApplyConfiguration(new DepartmentManagerEntityConfiguration());
            }
        }
    }
}
