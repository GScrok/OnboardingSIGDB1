using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces.Repositories.Generic;

namespace OnboardingSIGDB1.Domain.Interfaces.Repositories;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<List<Role>> GetByDescription(string description);

    Task<Role> GetByExactDescription(string description);
}