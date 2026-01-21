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
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public EmployeeService(IEmployeeRepository employeeRepository,
                               IMapper mapper,
                               INotificator notificator,
                               IUnitOfWork uow) : base(notificator)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task Add(EmployeeDto employeeDto)
        {
            if (!ExecuteValidation(new EmployeeDtoValidator(), employeeDto)) return;

            string cpfClean = StringUtils.RemoveMask(employeeDto.Cpf);

            Employee? existingEmployee = await _employeeRepository.GetByCpf(cpfClean);
            if (existingEmployee != null)
            {
                Notify("Já existe um funcionário cadastrado com este CPF.");
                return;
            }

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
                Employee? employeeWithCpf = await _employeeRepository.GetByCpf(cpfClean);
                if (employeeWithCpf != null)
                {
                    Notify("Já existe outro funcionário com este CPF.");
                    return;
                }
            }

            existingEmployee.Name = employeeDto.Name;
            existingEmployee.Cpf =  cpfClean;
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
    }