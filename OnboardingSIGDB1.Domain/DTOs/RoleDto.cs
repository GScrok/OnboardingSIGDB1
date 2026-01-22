using Swashbuckle.AspNetCore.Annotations;

namespace OnboardingSIGDB1.Domain.DTOs;

public class RoleDto
{
    [SwaggerSchema(ReadOnly = true)]
    public int? Id { get; set; }
    public string Description { get; set; }
}