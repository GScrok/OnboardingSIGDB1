using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;

namespace OnboardingSIGDB1.Domain.Interfaces.Repositories.ReadRepositories;

public interface IEmployeeReadRepository
{
    Task<EmployeeDto> GetById(int id);
    Task<IEnumerable<EmployeeDto>> GetAll();
    Task<IEnumerable<EmployeeDto>> GetByFilters(EmployeeFilter filter);
}