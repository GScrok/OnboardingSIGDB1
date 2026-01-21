using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnboardingSIGDB1.Domain.Entities;

namespace OnboardingSIGDB1.Data.Mappings;

public class EmployeeMapping : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Funcionario");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Cpf)
            .HasMaxLength(11)
            .IsRequired();
        
        builder.Property(e => e.HiringDate)
            .IsRequired(false);
        
        builder.HasOne(e => e.Company)
            .WithMany()
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}