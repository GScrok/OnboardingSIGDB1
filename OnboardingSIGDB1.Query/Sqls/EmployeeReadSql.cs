using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces.Repositories.ReadRepositories;
using SqlKata.Execution;

namespace OnboardingSIGDB1.Query.Sqls;

public class EmployeeReadSql : IEmployeeReadRepository
{
    private readonly QueryFactory _factory;
    
    public EmployeeReadSql(QueryFactory factory)
    {
        _factory = factory;
    }

    public async Task<EmployeeDto> GetById(int id)
    {
        return await _factory.Query("Funcionario")
            .Where("id", id)
            .Select("Funcionario.*")
            .SelectRaw("""
                       (
                           SELECT TOP 1 C.Description
                           FROM FuncionarioCargo AS FC
                           JOIN Cargo AS C ON FC.RoleId = C.Id
                           WHERE FC.EmployeeId = Funcionario.Id
                           ORDER BY FC.StartDate DESC
                       ) AS LastRole 
                       """)
            .FirstOrDefaultAsync<EmployeeDto>();
    }

    public async Task<IEnumerable<EmployeeDto>> GetAll()
    {
        return await _factory.Query("Funcionario")
            .Select("Funcionario.*")
            .SelectRaw("""
                       (
                           SELECT TOP 1 C.Description
                           FROM FuncionarioCargo AS FC
                           JOIN Cargo AS C ON FC.RoleId = C.Id
                           WHERE FC.EmployeeId = Funcionario.Id
                           ORDER BY FC.StartDate DESC
                       ) AS LastRole 
                       """)
            .GetAsync<EmployeeDto>();
    }
    
    public async Task<IEnumerable<EmployeeDto>> GetByFilters(EmployeeFilter filter)
    {
        var query = _factory.Query("Funcionario");

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.WhereContains("Name", filter.Name);
        }

        if (!string.IsNullOrEmpty(filter.Cpf))
        {
            query = query.Where("Cpf", filter.Cpf);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where("HiringDate", filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where("HiringDate", filter.EndDate.Value);
        }

        return await query
            .Select("Funcionario.*")
            .SelectRaw("""
                       (
                           SELECT TOP 1 C.Description
                           FROM FuncionarioCargo AS FC
                           JOIN Cargo AS C ON FC.RoleId = C.Id
                           WHERE FC.EmployeeId = Funcionario.Id
                           ORDER BY FC.StartDate DESC
                       ) AS LastRole 
                       """)
            .GetAsync<EmployeeDto>();
    }
}