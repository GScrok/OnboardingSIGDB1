using OnboardingSIGDB1.Domain.Entities;

namespace OnboardingSIGDB1.Domain.Interfaces.Repositories;

public interface IEmployeeRoleRepository : IDisposable
{
    Task Add(EmployeeRole employeeRole);
    Task<EmployeeRole> GetByKeys(int employeeId, int roleId);
}