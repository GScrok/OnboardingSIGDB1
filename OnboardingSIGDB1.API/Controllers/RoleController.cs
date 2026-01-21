using Microsoft.AspNetCore.Mvc;
using OnboardingSIGDB1.API.Controllers.Base;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Interfaces.Services;

namespace OnboardingSIGDB1.API.Controllers;

[Route("api/cargos")]
public class RolesController : MainController
{
    private readonly IRoleService _roleService;

    public RolesController(INotificator notificator, IRoleService roleService) : base(notificator)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<RoleDto> result = await _roleService.GetAll();

        return CustomResponse(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        RoleDto? result = await _roleService.GetById(id);

        if (result == null) return NotFound();

        return CustomResponse(result);
    }

    [HttpGet("pesquisar")]
    public async Task<IActionResult> Search([FromQuery] RoleFilter filter)
    {
        IEnumerable<RoleDto> result = await _roleService.GetByFilter(filter);

        return CustomResponse(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleDto dto)
    {
        if (!ModelState.IsValid) return NotifyErrorModelInvalid(ModelState);

        await _roleService.Add(dto);

        return CustomResponse(dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoleUpdateDto dto)
    {
        if (!ModelState.IsValid) return NotifyErrorModelInvalid(ModelState);

        await _roleService.Update(id, dto);

        return CustomResponse(dto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _roleService.Remove(id);

        return CustomResponse();
    }
}