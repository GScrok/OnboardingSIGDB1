using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Data.Repositories.Generic;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces.Repositories;

namespace OnboardingSIGDB1.Data.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(DataContext context) : base(context) { }

    public async Task<List<Role>> GetByDescription(string description)
    {
        IQueryable<Role> query = DbSet.AsQueryable();

        if (!string.IsNullOrEmpty(description))
        {
            query = query.Where(r => r.Description.Contains(description));
        }

        return await query.ToListAsync();
    }
    
    public async Task<Role> GetByExactDescription(string description)
    {
        IQueryable<Role> query = DbSet.AsQueryable();

        if (!string.IsNullOrEmpty(description))
        {
            query = query.Where(r => r.Description.Equals(description));
        }

        return await query.FirstOrDefaultAsync();
    }
}