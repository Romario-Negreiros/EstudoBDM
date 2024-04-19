using EstudoBDM.Configs;
using EstudoBDM.DTOs;
using EstudoBDM.Models;

namespace EstudoBDM.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> Add(EmployeeDTOs.AddEmployeeDTO employee);

        Task<Employee[]> GetAll();

        Task<Employee?> GetById(int id);
    }
    public class EmployeeRepository(DatabaseConnection DbCon) : IEmployeeRepository
    {
        public DatabaseConnection DbCon = DbCon;

        // ASP.CORE V6 => V8 => Use primary constructor as done above, below code is how it was in asp.core v6 (not mandatory)
        // public EmployeeRepository(DatabaseConnection DbCon)
        // {
        //     this.DbCon = DbCon;
        // }

        #region Add employee
        public async Task<Employee> Add(EmployeeDTOs.AddEmployeeDTO employee)
        {
            var employeeProps = employee.GetType().GetProperties();

            var procParams = employeeProps.Select(p => p.GetValue(employee)!).ToArray();

            var newEmployee = await DbCon.CallProcedureSingle<Employee>("sp_ins_employee", procParams);

            return newEmployee;
        }
        #endregion Add employee

        #region Get all employees
        public async Task<Employee[]> GetAll()
        {
            return await DbCon.CallProcedureList<Employee>("sp_get_all_employees");
        }
        #endregion Get all employees

        #region Get employee by id
        public async Task<Employee?> GetById(int id)
        {
            var result = await DbCon.CallProcedureSingle<Employee>("sp_get_employee_by_id", id);

            return result;
        }
        #endregion Get employee by id
    }
}
