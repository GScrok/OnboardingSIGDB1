using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Data.Repositories.Generic;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces.Repositories;

namespace OnboardingSIGDB1.Data.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(DataContext context) : base(context) { }

    public async Task<List<Role>> GetByDescription(RoleFilter filter)
    {
        IQueryable<Role> query = DbSet.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Description))
        {
            query = query.Where(r => r.Description.Contains(filter.Description));
        }

        return await query.ToListAsync();
    }
}