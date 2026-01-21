namespace OnboardingSIGDB1.Domain.DTOs;

public class CompanyUpdateDto
{
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime? FoundationDate { get; set; }
}