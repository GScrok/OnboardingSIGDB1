using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnboardingSIGDB1.Domain.Entities;

namespace OnboardingSIGDB1.Data.Mappings
{
    public class EmployeeRoleMapping : IEntityTypeConfiguration<EmployeeRole>
    {
        public void Configure(EntityTypeBuilder<EmployeeRole> builder)
        {
            builder.ToTable("FuncionarioCargo");
            
            builder.HasKey(er => new { er.EmployeeId, er.RoleId });

            builder.Property(er => er.StartDate)
                .IsRequired();
            
            builder.HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(er => er.Role)
                .WithMany()
                .HasForeignKey(er => er.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}