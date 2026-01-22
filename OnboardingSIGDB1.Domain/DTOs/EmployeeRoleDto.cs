using Swashbuckle.AspNetCore.Annotations;

namespace OnboardingSIGDB1.Domain.DTOs;

public class EmployeeRoleDto
{
    public EmployeeRoleDto(int roleId, DateTime startDate, string? roleName)
    {
        RoleId = roleId;
        StartDate = startDate;
        RoleName = roleName;

    }
    public int RoleId { get; set; }
    public DateTime StartDate { get; set; }
    [SwaggerSchema(ReadOnly = true)]
    public string? RoleName { get; set; }
}