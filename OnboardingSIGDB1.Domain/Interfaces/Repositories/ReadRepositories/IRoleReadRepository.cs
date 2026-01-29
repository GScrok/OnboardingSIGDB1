using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Filters;

namespace OnboardingSIGDB1.Domain.Interfaces.Repositories.ReadRepositories;

public interface IRoleReadRepository
{
    Task<RoleDto> GetById(int id);
    Task<IEnumerable<RoleDto>> GetAll();
    Task<IEnumerable<RoleDto>> GetByFilters(RoleFilter filter);
}