using Microsoft.AspNetCore.Mvc;
using OnboardingSIGDB1.API.Controllers.Base;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;

namespace OnboardingSIGDB1.API.Controllers
{
    [Route("api/empresas")]
    public class CompaniesController : MainController
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(INotificator notificator, ICompanyService companyService) : base(notificator)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Retorna todas as empresas
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CompanyDto> companies = await _companyService.GetAll();
            return CustomResponse(companies);
        }

        /// <summary>
        /// Retorna uma empresa pelo ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            CompanyDto? company = await _companyService.GetById(id);
            
            if (company == null) return NotFound();
            
            return CustomResponse(company);
        }

        /// <summary>
        /// Pesquisa empresas com filtros
        /// </summary>
        [HttpGet("pesquisar")]
        public async Task<IActionResult> Search([FromQuery] CompanyFilter filter)
        {
            if (!ModelState.IsValid) return NotifyErrorModelInvalid(ModelState);
            
            IEnumerable<CompanyDto> companies = await _companyService.GetByFilters(filter);
                
            return CustomResponse(companies);
        }

        /// <summary>
        /// Cadastra uma nova empresa
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyDto companyDto)
        {
            if (!ModelState.IsValid) return NotifyErrorModelInvalid(ModelState);

            await _companyService.Add(companyDto);

            return CustomResponse(companyDto);
        }

        /// <summary>
        /// Atualiza uma empresa existente
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyUpdateDto companyDto)
        {
            if (!ModelState.IsValid) return NotifyErrorModelInvalid(ModelState);

            await _companyService.Update(id, companyDto);

            return CustomResponse(companyDto);
        }

        /// <summary>
        /// Remove uma empresa
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _companyService.Remove(id);
            return CustomResponse();
        }
    }
}