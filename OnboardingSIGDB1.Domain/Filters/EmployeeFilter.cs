namespace OnboardingSIGDB1.Domain.Filters;

public class EmployeeFilter
{
    public string? Name { get; set; }
    public string? Cpf { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}