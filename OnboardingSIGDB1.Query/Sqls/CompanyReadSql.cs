using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces.Repositories.ReadRepositories;
using SqlKata.Execution;

namespace OnboardingSIGDB1.Query.Sqls;

public class CompanyReadSql : ICompanyReadRepository
{
    private readonly QueryFactory _factory;
    
    public CompanyReadSql(QueryFactory factory)
    {
        _factory = factory;
    }

    public async Task<CompanyDto> GetById(int id)
    {
        return await _factory.Query("Empresa")
            .Where("id", id)
            .FirstOrDefaultAsync<CompanyDto>();
    }

    public async Task<IEnumerable<CompanyDto>> GetAll()
    {
        return await _factory.Query("Empresa")
            .GetAsync<CompanyDto>();
    }
    
    public async Task<IEnumerable<CompanyDto>> GetByFilters(CompanyFilter filter)
    {
        var query = _factory.Query("Empresa");

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.WhereContains("Name", filter.Name);
        }

        if (!string.IsNullOrEmpty(filter.Cnpj))
        {
            query = query.Where("Cnpj", filter.Cnpj);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where("FoundationDate", filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where("FoundationDate", filter.EndDate.Value);
        }

        return await query.GetAsync<CompanyDto>();
    }
}