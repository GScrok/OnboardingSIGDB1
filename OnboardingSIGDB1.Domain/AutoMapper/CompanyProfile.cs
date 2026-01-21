using AutoMapper;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Utils;

namespace OnboardingSIGDB1.Domain.AutoMapper;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<Company, CompanyDto>();

        CreateMap<CompanyDto, Company>()
            .ConstructUsing(d => new Company(
                d.Name,
                StringUtils.RemoveMask(d.Cnpj),
                d.FoundationDate
            ))
            .ForMember(dest => dest.Cnpj, opt => opt.Ignore());
    }
}