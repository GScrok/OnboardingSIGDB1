using System.ComponentModel.DataAnnotations;

namespace OnboardingSIGDB1.Domain.Filters;

public class CompanyFilter
{
    public string? Name { get; set; }
    public string? Cnpj { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}