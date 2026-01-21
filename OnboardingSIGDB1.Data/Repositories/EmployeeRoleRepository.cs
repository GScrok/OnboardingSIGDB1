using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Interfaces.Repositories;

namespace OnboardingSIGDB1.Data.Repositories;

public class EmployeeRoleRepository : IEmployeeRoleRepository
{
    protected readonly DataContext _context;
    protected readonly DbSet<EmployeeRole> _dbSet;

    public EmployeeRoleRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<EmployeeRole>();
    }

    public async Task Add(EmployeeRole employeeRole)
    {
        await _dbSet.AddAsync(employeeRole);
    }

    public async Task<EmployeeRole> GetByKeys(int employeeId, int roleId)
    {
        return await _dbSet.FindAsync(employeeId, roleId);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}