#pragma warning disable IDE1006

using EstudoBDM.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstudoBDM.Models
{
    public interface IEmployee
    {
        int id { get; }
        string name { get; }
        string age { get; }
        string? photo { get; }
    }

    [Table("EMPLOYEE")]
    public class Employee
    {
        [Key]
        public int id { get; private set; }

        public string name { get; private set; }

        public int age {  get; private set; }

        public string? photo {  get; private set; }

        public Employee(string name, int age, string? photo)
        {
            this.name  = name;
            this.age   = age;
            this.photo = photo;
        }

        public Employee(EmployeeDTOs.AddEmployeeDTO addEmployee)
        {
            // Nullable fields have been already checked here

            name  = addEmployee.name!;
            age   = (int)addEmployee.age!;
            photo = addEmployee.photo;
        }
    }
}
