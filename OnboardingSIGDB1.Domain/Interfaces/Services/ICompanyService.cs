using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Filters;

namespace OnboardingSIGDB1.Domain.Interfaces.Services
{
    public interface ICompanyService : IDisposable
    {
        Task Add(CompanyDto companyDto);
        Task Update(int id, CompanyUpdateDto companyDto);
        Task Remove(int id);
        Task<IEnumerable<CompanyDto>> GetAll();
        Task<CompanyDto> GetById(int id);
        Task<IEnumerable<CompanyDto>> GetByFilters(CompanyFilter filter);
    }
}