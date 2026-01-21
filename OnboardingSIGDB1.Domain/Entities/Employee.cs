using OnboardingSIGDB1.Domain.Entities.Base;

namespace OnboardingSIGDB1.Domain.Entities;

public class Employee : BaseEntity
{
    public Employee(string name, string cpf, DateTime? hiringDate, int? companyId)
    {
        Name = name;
        Cpf = cpf;
        HiringDate = hiringDate;
        CompanyId = companyId;
    }

    public string Name { get; set; }
    public string Cpf { get; set; }
    public DateTime? HiringDate { get; set; }
    public int? CompanyId { get; private set; }
    
    public virtual Company Company { get; set; }
}