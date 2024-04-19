using EstudoBDM.Controllers;
using EstudoBDM.Infraestructure;
using EstudoBDM.Models;
using EstudoBDM.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using EstudoBDM.Configs;
using Microsoft.EntityFrameworkCore;

namespace EstudoBDM.Tests
{
    public class EstudoBDMTest
    {
        private EmployeeController EmployeeController { get; set; }
        private IUnitOfWork Uof { get; set; }

        public EstudoBDMTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<DatabaseConnection>(options =>
            {
                string connectionString = $"Server={Environment.GetEnvironmentVariable("Server")};" +
                                          $"Database={Environment.GetEnvironmentVariable("Database")};" +
                                          $"User={Environment.GetEnvironmentVariable("User")};" +
                                          $"Password={Environment.GetEnvironmentVariable("Password")};";

                connectionString = "Server=localhost;Database=estudobdm;User=romario;Password=123456";

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var serviceProvider = services.BuildServiceProvider();

            Uof = serviceProvider.GetService<IUnitOfWork>()!;

            EmployeeController = new EmployeeController(Uof);
        }

        [Fact]
        public void POST_Employee_ReturnsCREATEDNewEmployee()
        {
            // Arrange
            EmployeeDTOs.AddEmployeeDTO newEmployee = new EmployeeDTOs.AddEmployeeDTO
            {
                name = "Test",
                age = 30,
                photo = "b64"
            };

            // Act
            var result = EmployeeController.Add(newEmployee);
            var createdResult = result as CreatedResult;

            // Assert
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.IsType<Employee>(createdResult.Value);
        }
    }
}