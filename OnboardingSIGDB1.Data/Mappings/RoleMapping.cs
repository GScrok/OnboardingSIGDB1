using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnboardingSIGDB1.Domain.Entities;

namespace OnboardingSIGDB1.Data.Mappings;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Cargo");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Description)
            .HasMaxLength(250)
            .IsRequired();
    }
}