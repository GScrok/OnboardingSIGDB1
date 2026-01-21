using AutoMapper;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Entities;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Repositories;
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
    private readonly ICompanyRepository _companyRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEmployeeRoleRepository _employeeRoleRepository;

    public EmployeeService(IUnitOfWork uow,
        INotificator notificator,
        IMapper mapper,
        IEmployeeRepository employeeRepository,
        ICompanyRepository companyRepository,
        IRoleRepository roleRepository,
        IEmployeeRoleRepository employeeRoleRepository
    ) : base(notificator)
    {
        _mapper = mapper;
        _uow = uow;
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
        _roleRepository = roleRepository;
        _employeeRoleRepository =  employeeRoleRepository;
    }

    public async Task Add(EmployeeDto employeeDto)
    {
        if (!ExecuteValidation(new EmployeeDtoValidator(), employeeDto)) return;

        if (!await ExistsCompany(employeeDto.CompanyId)) return;

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

        existingEmployee.Name = employeeDto.Name;
        existingEmployee.Cpf = cpfClean;
        existingEmployee.HiringDate = employeeDto.HiringDate;

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
        List<Employee> list = await _employeeRepository.GetAll();
        return _mapper.Map<IEnumerable<EmployeeDto>>(list);
    }

    public async Task<EmployeeDto> GetById(int id)
    {
        Employee entity = await _employeeRepository.GetById(id);
        return _mapper.Map<EmployeeDto>(entity);
    }

    public async Task<IEnumerable<EmployeeDto>> GetByFilters(EmployeeFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Cpf))
        {
            filter.Cpf = StringUtils.RemoveMask(filter.Cpf);
        }

        List<Employee> list = await _employeeRepository.GetByFilters(filter);
        return _mapper.Map<IEnumerable<EmployeeDto>>(list);
    }

    public void Dispose()
    {
        _employeeRepository?.Dispose();
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
}   