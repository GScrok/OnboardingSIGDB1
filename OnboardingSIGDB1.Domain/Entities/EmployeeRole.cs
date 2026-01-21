using OnboardingSIGDB1.Domain.Entities.Base;

namespace OnboardingSIGDB1.Domain.Entities;

public class EmployeeRole : BaseEntity
{
    public EmployeeRole(int employeeId, int roleId, DateTime startDate)
    {
        EmployeeId = employeeId;
        RoleId = roleId;
        StartDate = startDate;
    }

    public int EmployeeId { get; private set; }
    public int RoleId { get; private set; }
    public DateTime StartDate { get; private set; }

    
    public virtual Employee Employee { get; private set; }
    public virtual Role Role { get; private set; }
}