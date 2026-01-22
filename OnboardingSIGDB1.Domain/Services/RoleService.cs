using AutoMapper;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Repositories;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Services.Base;
using OnboardingSIGDB1.Domain.Validators;

namespace OnboardingSIGDB1.Domain.Services;

public class RoleService : BaseService, IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public RoleService(IRoleRepository roleRepository,
        IMapper mapper,
        INotificator notificator,
        IUnitOfWork uow) : base(notificator)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task Add(RoleDto roleDto)
    {
        if (!ExecuteValidation(new RoleDtoValidator(), roleDto)) return;

        Role? existingRoleByDescription = await _roleRepository.GetByExactDescription(roleDto.Description);
        if (existingRoleByDescription != null)
        {
            Notify("Já existe um cargo com a mesma descrição no sistema.");
            return;
        }
        
        Role? role = _mapper.Map<Role>(roleDto);

        await _roleRepository.Add(role);

        if (!await _uow.Commit()) Notify("Erro ao persistir cargo.");
    }

    public async Task Update(int id, RoleUpdateDto roleDto)
    {
        if (!ExecuteValidation(new RoleUpdateDtoValidator(), roleDto)) return;

        if (id == null)
        {
            Notify("ID inválido.");
            return;
        }

        Role? existingRole = await _roleRepository.GetById(id);
        if (existingRole == null)
        {
            Notify("Cargo não encontrado.");
            return;
        }

        if (existingRole.Description != roleDto.Description)
        {
            Role? existingRoleByDescription = await _roleRepository.GetByExactDescription(roleDto.Description);
            if (existingRoleByDescription != null)
            {
                Notify("Já existe um cargo com a mesma descrição no sistema.");
                return;
            }
        }

        _mapper.Map(roleDto, existingRole);

        _roleRepository.Update(existingRole);

        if (!await _uow.Commit())
        {
            Notify("Erro ao atualizar cargo.");
        }
    }

    public async Task Remove(int id)
    {
        Role? role = await _roleRepository.GetById(id);
        if (role == null)
        {
            Notify("Cargo não encontrado.");
            return;
        }

        _roleRepository.Remove(role);
        if (!await _uow.Commit())
        {
            Notify("Erro ao remover cargo.");
        }
    }

    public async Task<IEnumerable<RoleDto>> GetAll()
    {
        List<Role> list = await _roleRepository.GetAll();
        return _mapper.Map<IEnumerable<RoleDto>>(list);
    }

    public async Task<RoleDto> GetById(int id)
    {
        Role entity = await _roleRepository.GetById(id);

        return _mapper.Map<RoleDto>(entity);
    }

    public async Task<IEnumerable<RoleDto>> GetByFilter(RoleFilter filter)
    {
        List<Role> list = await _roleRepository.GetByDescription(filter.Description);

        return _mapper.Map<IEnumerable<RoleDto>>(list);
    }

    public void Dispose()
    {
        _roleRepository?.Dispose();
    }
}