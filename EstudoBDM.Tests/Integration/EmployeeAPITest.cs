using EstudoBDM.Controllers;
using EstudoBDM.Models;
using EstudoBDM.DTOs;
using EstudoBDM.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace EstudoBDM.Tests.Integration
{
    public class EmployeeAPITest(ConfigsFixture configsFixture) : IClassFixture<ConfigsFixture>
    {
        private EmployeeController EmployeeController { get; set; } = new EmployeeController(configsFixture.Uof);

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
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.IsType<Employee>(createdResult.Value);
        }

        [Fact]
        public async void GET_AllEmployees_ReturnsOKListOfEmployees()
        {
            // Arrange

            // Act
            var result = await EmployeeController.GetAll();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsType<Employee[]>(okResult.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void GET_EmployeeById_ReturnsOkSingleEmployee(int id)
        {
            // Act
            var result = await EmployeeController.GetById(id);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsType<Employee>(okResult.Value);
        }

        [Theory]
        [InlineData(564684)]
        [InlineData(2309432)]
        [InlineData(1921982)]
        public async void GET_EmployeeById_ReturnsNotFoundErrorObj(int id)
        {
            // Act
            var result = await EmployeeController.GetById(id);
            var notFoundResult = result as NotFoundObjectResult;

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.IsType<ApiResponsesDTOs.ExceptionDTO>(notFoundResult.Value);
        }
    }
}