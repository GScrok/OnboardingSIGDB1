using Microsoft.AspNetCore.Mvc;
using OnboardingSIGDB1.API.Controllers.Base;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;

namespace OnboardingSIGDB1.API.Controllers;

[Route("api/funcionarios")]
public class EmployeesController : MainController
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(INotificator notificator, IEmployeeService employeeService) : base(notificator)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// Retorna todos os funcionários
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<EmployeeDto> result = await _employeeService.GetAll();
        return CustomResponse(result);
    }

    /// <summary>
    /// Retorna um funcionário pelo ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        EmployeeDto? result = await _employeeService.GetById(id);
        if (result == null) return NotFound();

        return CustomResponse(result);
    }

    /// <summary>
    /// Pesquisa funcionários com filtros
    /// </summary>
    [HttpGet("pesquisar")]
    public async Task<IActionResult> Search([FromQuery] EmployeeFilter filter)
    {
        IEnumerable<EmployeeDto> result = await _employeeService.GetByFilters(filter);

        return CustomResponse(result);
    }

    /// <summary>
    /// Cadastra um novo funcionário
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeDto dto)
    {
        if (!ModelState.IsValid) return NotifyErrorModelInvalid(ModelState);

        await _employeeService.Add(dto);

        return CustomResponse(dto);
    }

    /// <summary>
    /// Atualiza um funcionário existente
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto dto)
    {
        if (!ModelState.IsValid) return NotifyErrorModelInvalid(ModelState);

        await _employeeService.Update(id, dto);

        return CustomResponse(dto);
    }

    /// <summary>
    /// Remove um funcionário
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeService.Remove(id);
        return CustomResponse();
    }
    
    [HttpPost("{id:int}/cargos")]
    public async Task<IActionResult> AddRole(int id, [FromBody] EmployeeRoleDto dto)
    {
        if (!ModelState.IsValid) return NotifyErrorModelInvalid(ModelState);

        await _employeeService.LinkRole(id, dto);

        return CustomResponse(dto);
    }

    [HttpGet("{id:int}/cargos")]
    public async Task<IActionResult> HistoryEmployeeRole(int id)
    {
        List<EmployeeRoleDto> employeeRoles = await _employeeService.GetEmployeeRoles(id);
        
        return CustomResponse(employeeRoles);
    }
    
    
}