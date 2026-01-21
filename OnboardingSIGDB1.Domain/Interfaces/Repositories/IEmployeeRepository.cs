using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces.Repositories.Generic;

namespace OnboardingSIGDB1.Domain.Interfaces.Repositories;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<List<Employee>> GetByFilters(EmployeeFilter filter);
    Task<Employee?> GetByCpf(string cpf);
}