using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Data.Repositories.Generic;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces.Repositories;

namespace OnboardingSIGDB1.Data.Repositories;

public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
{
    public CompanyRepository(DataContext context) : base(context)
    {
    }

    public async Task<List<Company>> GetByFilters(CompanyFilter filter)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(c => c.Name.Contains(filter.Name));
        }

        if (!string.IsNullOrEmpty(filter.Cnpj))
        {
            query = query.Where(c => c.Cnpj == filter.Cnpj);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(c => c.FoundationDate >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(c => c.FoundationDate <= filter.EndDate.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Company> GetByCnpj(string cnpj)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Cnpj == cnpj);
    }
}