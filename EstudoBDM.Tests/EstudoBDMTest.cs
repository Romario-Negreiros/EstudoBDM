using EstudoBDM.Controllers;
using EstudoBDM.Infraestructure;
using EstudoBDM.Models;
using EstudoBDM.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using EstudoBDM.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

namespace EstudoBDM.Tests
{
    public class EstudoBDMTest
    {
        private EmployeeController EmployeeController { get; set; }
        private IUnitOfWork Uof { get; set; }

        public EstudoBDMTest()
        {
            var builder = WebApplication.CreateBuilder();

            var services = new ServiceCollection();

            services.AddDbContext<DatabaseConnection>(options =>
            {
                string connectionString = $"Server={builder.Configuration["Server"]};" +
                              $"Database={builder.Configuration["Database"]};" +
                              $"User={builder.Configuration["User"]};" +
                              $"Password={builder.Configuration["Password"]};";

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var serviceProvider = services.BuildServiceProvider();

            Uof = serviceProvider.GetService<IUnitOfWork>()!;

            EmployeeController = new EmployeeController(Uof);
        }

        [Fact]
        public async void POST_Employee_ReturnsCREATEDNewEmployee()
        {
            // Arrange
            EmployeeDTOs.AddEmployeeDTO newEmployee = new()
            {
                name = "Test",
                age = 30,
                photo = "b64"
            };

            // Act
            var result = await EmployeeController.Add(newEmployee);
            var createdResult = result as CreatedResult;

            // Assert
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.IsType<Employee>(createdResult.Value);
        }
    }
}