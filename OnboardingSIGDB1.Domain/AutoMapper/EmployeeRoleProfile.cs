using AutoMapper;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Entities;

namespace OnboardingSIGDB1.Domain.AutoMapper;

public class EmployeeRoleProfile : Profile
{
        public EmployeeRoleProfile()
        {
                CreateMap<EmployeeRole, EmployeeRoleDto>()
                        .ConstructUsing(x => new EmployeeRoleDto(
                                x.RoleId,
                                x.StartDate,
                                x.Role.Description
                        ));
        }
}