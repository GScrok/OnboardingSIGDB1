using AutoMapper;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Repositories;
using OnboardingSIGDB1.Domain.Interfaces.Repositories.ReadRepositories;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Services.Base;
using OnboardingSIGDB1.Domain.Utils;
using OnboardingSIGDB1.Domain.Validators;

namespace OnboardingSIGDB1.Domain.Services;

public class EmployeeService : BaseService, IEmployeeService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeReadRepository _employeeReadRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEmployeeRoleRepository _employeeRoleRepository;

    public EmployeeService(IUnitOfWork uow,
        INotificator notificator,
        IMapper mapper,
        IEmployeeRepository employeeRepository,
        IEmployeeReadRepository employeeReadRepository,
        ICompanyRepository companyRepository,
        IRoleRepository roleRepository,
        IEmployeeRoleRepository employeeRoleRepository
    ) : base(notificator)
    {
        _mapper = mapper;
        _uow = uow;
        _employeeRepository = employeeRepository;
        _employeeReadRepository = employeeReadRepository;
        _companyRepository = companyRepository;
        _roleRepository = roleRepository;
        _employeeRoleRepository =  employeeRoleRepository;
    }

    public async Task Add(EmployeeDto employeeDto)
    {
        if (!ExecuteValidation(new EmployeeDtoValidator(), employeeDto)) return;

        if (employeeDto.CompanyId != null)
        {
            if (!await ExistsCompany(employeeDto.CompanyId.Value)) return;
        }

        if (await IsCpfAlreadyRegistered(employeeDto.Cpf)) return;

        Employee? employee = _mapper.Map<Employee>(employeeDto);
        await _employeeRepository.Add(employee);

        if (!await _uow.Commit())
        {
            Notify("Erro ao persistir funcionário.");
        }
    }

    public async Task Update(int id, EmployeeUpdateDto employeeDto)
    {
        if (!ExecuteValidation(new EmployeeUpdateDtoValidator(), employeeDto)) return;

        if (id == null)
        {
            Notify("ID inválido.");
            return;
        }

        Employee? existingEmployee = await _employeeRepository.GetById(id);
        if (existingEmployee == null)
        {
            Notify("Funcionário não encontrado.");
            return;
        }

        string cpfClean = StringUtils.RemoveMask(employeeDto.Cpf);

        if (existingEmployee.Cpf != cpfClean)
        {
            if (await IsCpfAlreadyRegistered(employeeDto.Cpf)) return;
        }

        if (existingEmployee.CompanyId != null && existingEmployee.CompanyId != employeeDto.CompanyId)
        {
            Notify("Não é permitido alterar o vínculo de funcionário com empresa já registrada.");
            return;
        }
        
        _mapper.Map(employeeDto, existingEmployee);

        _employeeRepository.Update(existingEmployee);

        if (!await _uow.Commit())
        {
            Notify("Erro ao atualizar funcionário.");
        }
    }

    public async Task Remove(int id)
    {
        Employee? employee = await _employeeRepository.GetById(id);
        if (employee == null)
        {
            Notify("Funcionário não encontrado.");
            return;
        }

        _employeeRepository.Remove(employee);

        if (!await _uow.Commit())
        {
            Notify("Erro ao remover funcionário.");
        }
    }

    public async Task<IEnumerable<EmployeeDto>> GetAll()
    {
        return await _employeeReadRepository.GetAll();
    }

    public async Task<EmployeeDto> GetById(int id)
    {
        return await _employeeReadRepository.GetById(id);
    }

    public async Task<IEnumerable<EmployeeDto>> GetByFilters(EmployeeFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Cpf))
        {
            filter.Cpf = StringUtils.RemoveMask(filter.Cpf);
        }

        return await _employeeReadRepository.GetByFilters(filter);
    }

    private async Task<bool> ExistsCompany(int companyId)
    {
        Company? companyExists = await _companyRepository.GetById(companyId);
        if (companyExists == null)
        {
            Notify("A Empresa informada não existe.");
            return false;
        }

        return true;
    }

    async Task<bool> IsCpfAlreadyRegistered(string cpf)
    {
        string cpfClean = StringUtils.RemoveMask(cpf);

        Employee? existingEmployee = await _employeeRepository.GetByCpf(cpfClean);
        if (existingEmployee != null)
        {
            Notify("Já existe um funcionário cadastrado com este CPF.");
            return true;
        }

        return false;
    }

    public async Task LinkRole(int employeeId, EmployeeRoleDto dto)
    {
        Employee? employee = await _employeeRepository.GetById(employeeId);
        if (employee == null)
        {
            Notify("Funcionário não encontrado.");
            return;
        }

        Role? role = await _roleRepository.GetById(dto.RoleId);
        if (role == null)
        {
            Notify("Cargo não encontrado.");
            return;
        }

        if (employee.CompanyId == null)
        {
            Notify("Este funcionário não está vinculado a uma empresa.");
            return;
        }
        
        EmployeeRole? existingLink = await _employeeRoleRepository.GetByKeys(employeeId, dto.RoleId);
        if (existingLink != null)
        {
            Notify("Este funcionário já possui este cargo vinculado.");
            return;
        }

        EmployeeRole employeeRole = new EmployeeRole(employeeId, dto.RoleId, dto.StartDate);

        await _employeeRoleRepository.Add(employeeRole);

        if (!await _uow.Commit())
        {
            Notify("Erro ao vincular cargo ao funcionário.");
        }
    }

    public async Task<List<EmployeeRoleDto>> GetEmployeeRoles(int employeeId)
    {
        if (employeeId == null)
        {
            Notify("ID inválido.");
            return null;
        }

        Employee? existingEmployee = await _employeeRepository.GetById(employeeId);
        if (existingEmployee == null)
        {
            Notify("Funcionário não encontrado.");
            return null;
        }

        List<EmployeeRoleDto> employeeRolesDto = new();
        
        List<EmployeeRole> employeeRoles = await  _employeeRoleRepository.GetListByEmployeeId(employeeId);

        _mapper.Map(employeeRoles, employeeRolesDto);

        return employeeRolesDto;
    }
}   