using Xunit;
using Microsoft.EntityFrameworkCore;
using HCM.Data.Models;
using HCM.Web.ViewModels.Department;
using HCM.Services.Data.Tests.Mocks;

namespace HCM.Services.Data.Tests
{
    public class DepartmentServiceTests
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDepartments()
        {
            // Arrange
            var context = DatabaseMock.Instance;
            var departments = new List<Department>
        {
            new Department { Id = Guid.NewGuid(), Name = "Engineering" },
            new Department { Id = Guid.NewGuid(), Name = "Finance" },
            new Department { Id = Guid.NewGuid(), Name = "HR" }
        };

            await context.Departments.AddRangeAsync(departments);
            await context.SaveChangesAsync();

            var departmentService = new DepartmentService(context);

            // Act
            var result = await departmentService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(departments[0].Name, result.First().Name);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewDepartment()
        {
            // Arrange
            var context = DatabaseMock.Instance;
            var departmentService = new DepartmentService(context);
            var model = new DepartmentFormModel { Name = "Marketing" };

            // Act
            await departmentService.CreateAsync(model);

            // Assert
            var department = await context.Departments.FirstOrDefaultAsync(d => d.Name == "Marketing");
            Assert.NotNull(department);
            Assert.Equal("Marketing", department.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectDepartment()
        {
            // Arrange
            var context = DatabaseMock.Instance;
            var departmentId = Guid.NewGuid();
            var department = new Department { Id = departmentId, Name = "IT" };

            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();

            var departmentService = new DepartmentService(context);

            // Act
            var result = await departmentService.GetByIdAsync(departmentId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(departmentId.ToString(), result.Id);
            Assert.Equal("IT", result.Name);
        }

        [Fact]
        public async Task EditAsync_ShouldUpdateDepartment()
        {
            // Arrange
            var context = DatabaseMock.Instance;
            var departmentId = Guid.NewGuid();
            var department = new Department { Id = departmentId, Name = "Old Name" };

            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();

            var departmentService = new DepartmentService(context);
            var model = new DepartmentFormModel
            {
                Id = departmentId.ToString(),
                Name = "New Name"
            };

            // Act
            await departmentService.EditAsync(model);

            // Assert
            var updatedDepartment = await context.Departments.FindAsync(departmentId);
            Assert.NotNull(updatedDepartment);
            Assert.Equal("New Name", updatedDepartment.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveDepartmentAndRelatedEntities()
        {
            // Arrange
            var context = DatabaseMock.Instance;
            var departmentId = Guid.NewGuid();
            var managerId = Guid.NewGuid();

            var department = new Department
            {
                Id = departmentId,
                Name = "Department to Delete",
                Employees = new List<Employee>(),
                Managers = new List<DepartmentManager>()
            };

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                JobTitle = "Developer",
                DepartmentId = departmentId,
                Department = department
            };

            var departmentManager = new DepartmentManager
            {
                ManagerId = managerId,
                DepartmentId = departmentId,
                Department = department
            };

            department.Employees.Add(employee);
            department.Managers.Add(departmentManager);

            await context.Departments.AddAsync(department);
            await context.Employees.AddAsync(employee);
            await context.DepartmentsManagers.AddAsync(departmentManager);
            await context.SaveChangesAsync();

            var departmentService = new DepartmentService(context);

            // Act
            await departmentService.DeleteAsync(departmentId.ToString());

            // Assert
            var deletedDepartment = await context.Departments.FindAsync(departmentId);
            Assert.Null(deletedDepartment);

            var departmentEmployees = await context.Employees
                .Where(e => e.DepartmentId == departmentId)
                .ToListAsync();
            Assert.Empty(departmentEmployees);

            var departmentManagers = await context.DepartmentsManagers
                .Where(dm => dm.DepartmentId == departmentId)
                .ToListAsync();
            Assert.Empty(departmentManagers);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var context = DatabaseMock.Instance;
            var departmentService = new DepartmentService(context);
            var invalidId = Guid.NewGuid().ToString();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => departmentService.GetByIdAsync(invalidId));
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var context = DatabaseMock.Instance;
            var departmentService = new DepartmentService(context);
            var invalidId = Guid.NewGuid().ToString();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => departmentService.DeleteAsync(invalidId));
        }

        [Fact]
        public async Task EditAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var context = DatabaseMock.Instance;
            var departmentService = new DepartmentService(context);
            var model = new DepartmentFormModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Invalid Department"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => departmentService.EditAsync(model));
        }
    }
}