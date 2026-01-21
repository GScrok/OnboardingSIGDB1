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

namespace OnboardingSIGDB1.Domain.Services
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CompanyService(ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            INotificator notificator,
            IUnitOfWork uow) : base(notificator)
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task Add(CompanyDto companyDto)
        {
            Company? company = _mapper.Map<Company>(companyDto);

            if (!ExecuteValidation(new CompanyDtoValidator(), companyDto)) return;

            Company? existingCompany = await _companyRepository.GetByCnpj(company.Cnpj);
            if (existingCompany != null)
            {
                Notify("Já existe uma empresa cadastrada com este CNPJ.");
                return;
            }

            await _companyRepository.Add(company);

            if (!await _uow.Commit())
            {
                Notify("Houve um erro ao persistir os dados da empresa.");
            }
        }

        public async Task Update(int id, CompanyUpdateDto companyDto)
        {
            if (!ExecuteValidation(new CompanyUpdateDtoValidator(), companyDto)) return;

            if (id == null)
            {
                Notify("O ID da empresa não foi informado.");
                return;
            }

            Company? existingCompany = await _companyRepository.GetById(id);
            if (existingCompany == null)
            {
                Notify("Empresa não encontrada.");
                return;
            }

            string cleanCnpj = StringUtils.RemoveMask(companyDto.Cnpj);
            if (existingCompany.Cnpj != cleanCnpj)
            {
                Company? companyWithCnpj = await _companyRepository.GetByCnpj(cleanCnpj);
                if (companyWithCnpj != null)
                {
                    Notify("Já existe uma outra empresa cadastrada com este CNPJ.");
                    return;
                }
            }

            existingCompany.Name = companyDto.Name;
            existingCompany.Cnpj = cleanCnpj;
            existingCompany.FoundationDate = companyDto.FoundationDate;

            _companyRepository.Update(existingCompany);

            if (!await _uow.Commit())
            {
                Notify("Houve um erro ao atualizar os dados da empresa.");
            }
        }

        public async Task Remove(int id)
        {
            Company? company = await _companyRepository.GetById(id);
            if (company == null)
            {
                Notify("Empresa não encontrada.");
                return;
            }

            if (await _employeeRepository.HasEmployeeInCompany(id))
            {
                Notify("Não é possível excluir esta empresa pois ela possui funcionários vinculados.");
                return;
            }
            
            _companyRepository.Remove(company);

            if (!await _uow.Commit())
            {
                Notify("Houve um erro ao remover a empresa.");
            }
        }

        public async Task<IEnumerable<CompanyDto>> GetAll()
        {
            List<Company> companies = await _companyRepository.GetAll();
            return _mapper.Map<IEnumerable<CompanyDto>>(companies);
        }

        public async Task<CompanyDto> GetById(int id)
        {
            Company company = await _companyRepository.GetById(id);
            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<IEnumerable<CompanyDto>> GetByFilters(CompanyFilter filter)
        {
            if (!ExecuteValidation(new CompanyFilterValidator(), filter)) return null;

            if (!string.IsNullOrEmpty(filter.Cnpj))
            {
                filter.Cnpj = StringUtils.RemoveMask(filter.Cnpj);
            }

            List<Company> companies = await _companyRepository.GetByFilters(filter);
            return _mapper.Map<IEnumerable<CompanyDto>>(companies);
        }

        public void Dispose()
        {
            _companyRepository?.Dispose();
        }
    }
}