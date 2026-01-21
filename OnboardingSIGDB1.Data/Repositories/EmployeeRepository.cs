using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Data.Repositories.Generic;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces.Repositories;

namespace OnboardingSIGDB1.Data.Repositories;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(DataContext context) : base(context) { }

    public async Task<List<Employee>> GetByFilters(EmployeeFilter filter)
    {
        IQueryable<Employee> query = DbSet.AsQueryable();
        
        query = query.Include(e => e.EmployeeRoles).ThenInclude(er => er.Role);

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(e => e.Name.Contains(filter.Name));

        if (!string.IsNullOrEmpty(filter.Cpf))
            query = query.Where(e => e.Cpf == filter.Cpf);

        if (filter.StartDate.HasValue)
            query = query.Where(e => e.HiringDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(e => e.HiringDate <= filter.EndDate.Value);

        return await query.ToListAsync();
    }

    public async Task<Employee?> GetByCpf(string cpf)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Cpf == cpf);
    }
    
    public async Task<bool> HasEmployeeInCompany(int companyId)
    {
        return await DbSet.AsNoTracking().AnyAsync(e => e.CompanyId == companyId);
    }
    
    public override async Task<Employee?> GetById(int id)
    {
        return await DbSet
            .AsNoTracking()
            .Include(e => e.EmployeeRoles)
            .ThenInclude(er => er.Role)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public override async Task<List<Employee>> GetAll()
    {
        return await DbSet
            .AsNoTracking()
            .Include(e => e.EmployeeRoles)
            .ThenInclude(er => er.Role)
            .ToListAsync();
    }
}