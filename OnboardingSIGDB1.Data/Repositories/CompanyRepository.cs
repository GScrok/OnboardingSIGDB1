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

    public async Task<Company> GetByCnpj(string cnpj)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Cnpj == cnpj);
    }
}