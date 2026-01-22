using Swashbuckle.AspNetCore.Annotations;

namespace OnboardingSIGDB1.Domain.DTOs;

public class CompanyDto
{
    [SwaggerSchema(ReadOnly = true)]
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime? FoundationDate { get; set; }
}