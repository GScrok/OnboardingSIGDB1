using AutoMapper;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Utils;

namespace OnboardingSIGDB1.Domain.AutoMapper;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>();

        CreateMap<EmployeeDto, Employee>()
            .ConstructUsing(d => new Employee(
                d.Name,
                StringUtils.RemoveMask(d.Cpf),
                d.HiringDate
            ))
            .ForMember(dest => dest.Cpf, opt => opt.Ignore());
    }
}