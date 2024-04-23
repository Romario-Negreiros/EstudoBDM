using EstudoBDM.Infraestructure;
using EstudoBDM.Repositories;
using EstudoBDM.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EstudoBDM.Controllers
{
    [ApiController]
    [Route("/api/v1/employee")]
    public class EmployeeController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _uof = unitOfWork;
        private readonly IEmployeeRepository _employeeRepository = unitOfWork.EmployeeRepository;

        [HttpPost]
        [Authorize]
        [ScopeRequirement(claimValue: "write")]
        public async Task<IActionResult> Add([FromBody] EmployeeDTOs.AddEmployeeDTO addEmployee)
        {
            try
            {
                if (addEmployee.name == null)
                {
                    return BadRequest("Name cannot be empty!");
                }

                if (addEmployee.age == null)
                {
                    return BadRequest("Age cannot be empty!");
                }

                var newEmployee = await _employeeRepository.Add(addEmployee);

                _uof.Commit();

                return Created(uri: "Url to get created resource", newEmployee);

            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponsesDTOs.ExceptionDTO(ex.Message))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet]
        [Authorize]
        [ScopeRequirement(claimValue: "read")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await _employeeRepository.GetAll();

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponsesDTOs.ExceptionDTO(ex.Message))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet("externo")]
        [Authorize]
        public async Task<IActionResult> ExternalGetAll()
        {
            return await GetAll();
        }


        [HttpGet("{id}")]
        [Authorize]
        [ScopeRequirement(claimValue: "read")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var employee = await _employeeRepository.GetById(id);

                if (employee == null)
                {
                    return NotFound(new ApiResponsesDTOs.ExceptionDTO("Employee not found!"));
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponsesDTOs.ExceptionDTO(ex.Message))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
