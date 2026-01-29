using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces.Repositories.ReadRepositories;
using SqlKata.Execution;

namespace OnboardingSIGDB1.Query.Sqls;

public class RoleReadSql : IRoleReadRepository
{
    private readonly QueryFactory _factory;
    
    public RoleReadSql(QueryFactory factory)
    {
        _factory = factory;
    }

    public async Task<RoleDto> GetById(int id)
    {
        return await _factory.Query("Cargo")
            .Where("id", id)
            .FirstOrDefaultAsync<RoleDto>();
    }

    public async Task<IEnumerable<RoleDto>> GetAll()
    {
        return await _factory.Query("Cargo")
            .GetAsync<RoleDto>();
    }
    
    public async Task<IEnumerable<RoleDto>> GetByFilters(RoleFilter filter)
    {
        var query = _factory.Query("Cargo");

        if (!string.IsNullOrEmpty(filter.Description))
        {
            query = query.WhereContains("Description",  filter.Description);
        }

        return await query.GetAsync<RoleDto>();
    }
}