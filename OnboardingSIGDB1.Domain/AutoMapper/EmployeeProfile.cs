using AutoMapper;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Utils;

namespace OnboardingSIGDB1.Domain.AutoMapper;
public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.LastRole, opt => opt.MapFrom(src => 
                    src.EmployeeRoles
                        .OrderByDescending(er => er.StartDate)
                        .Select(er => er.Role.Description)
                        .FirstOrDefault()
            ));

        
        CreateMap<EmployeeDto, Employee>()
            .ConstructUsing(d => new Employee(
                d.Name,
                StringUtils.RemoveMask(d.Cpf), 
                d.HiringDate,
                d.CompanyId
            ))
            .ForMember(dest => dest.Cpf, opt => opt.Ignore());
        
        CreateMap<EmployeeUpdateDto, Employee>()
            .ForMember(dest => dest.Cpf, opt => 
                opt.MapFrom(src => StringUtils.RemoveMask(src.Cpf)));
    }
}