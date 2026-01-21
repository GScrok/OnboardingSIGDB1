using OnboardingSIGDB1.Domain.Entities.Base;

namespace OnboardingSIGDB1.Domain.Entities;

public class Company : BaseEntity
{
    public Company(string name, string cnpj,  DateTime? foundationDate)
    {
        Name = name;
        Cnpj = cnpj;
        FoundationDate = foundationDate;
    }
    
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime? FoundationDate { get; set; }
}