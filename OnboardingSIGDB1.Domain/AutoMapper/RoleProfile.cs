using AutoMapper;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Entities;

namespace OnboardingSIGDB1.Domain.AutoMapper;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>();

        CreateMap<RoleDto, Role>()
            .ConstructUsing(d => new Role(
                d.Description
            ));
    }
}