using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;

namespace OnboardingSIGDB1.Domain.Interfaces.Repositories.ReadRepositories;

public interface ICompanyReadRepository
{
    Task<CompanyDto> GetById(int id);
    Task<IEnumerable<CompanyDto>> GetAll();
    Task<IEnumerable<CompanyDto>> GetByFilters(CompanyFilter filter);
}