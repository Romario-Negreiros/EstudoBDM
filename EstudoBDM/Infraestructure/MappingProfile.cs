using AutoMapper;
using EstudoBDM.DTOs;
using EstudoBDM.Models;

namespace EstudoBDM.Infraestructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Adc aqui os possíveis mapeamentos entre model > dto 
            CreateMap<Employee, EmployeeDTOs.AddEmployeeDTO>().ReverseMap();
        }
    }
}
