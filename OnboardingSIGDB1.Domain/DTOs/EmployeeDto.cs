namespace OnboardingSIGDB1.Domain.DTOs;

public class EmployeeDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Cpf { get; set; }
    public DateTime? HiringDate { get; set; }
    public int CompanyId { get; set; }
    public string? LastRole { get; set; }
}