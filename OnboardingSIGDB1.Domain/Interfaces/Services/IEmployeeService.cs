using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Filters;

namespace OnboardingSIGDB1.Domain.Interfaces.Services;

public interface IEmployeeService : IDisposable
{
    Task Add(EmployeeDto employeeDto);
    Task Update(int id, EmployeeUpdateDto employeeDto);
    Task Remove(int id);
    Task<IEnumerable<EmployeeDto>> GetAll();
    Task<EmployeeDto> GetById(int id);
    Task<IEnumerable<EmployeeDto>> GetByFilters(EmployeeFilter filter);
    Task LinkRole(int employeeId, EmployeeRoleDto dto);
    Task<List<EmployeeRoleDto>> GetEmployeeRoles(int employeeId);
}