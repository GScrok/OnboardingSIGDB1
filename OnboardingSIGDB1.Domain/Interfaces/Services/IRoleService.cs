using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Filters;

namespace OnboardingSIGDB1.Domain.Interfaces.Services;

public interface IRoleService : IDisposable
{
    Task Add(RoleDto roleDto);
    Task Update(int id, RoleUpdateDto roleDto);
    Task Remove(int id);
    Task<IEnumerable<RoleDto>> GetAll();
    Task<RoleDto> GetById(int id);
    Task<IEnumerable<RoleDto>> GetByFilter(RoleFilter filter);
}