using Swashbuckle.AspNetCore.Annotations;

namespace OnboardingSIGDB1.Domain.DTOs;

public class EmployeeDto
{
    [SwaggerSchema(ReadOnly = true)]
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Cpf { get; set; }
    public DateTime? HiringDate { get; set; }
    public int? CompanyId { get; set; }
    [SwaggerSchema(ReadOnly = true)]
    public string? LastRole { get; set; }
}