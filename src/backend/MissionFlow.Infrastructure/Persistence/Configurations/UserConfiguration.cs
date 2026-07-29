using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MissionFlow.Domain.Entities;

namespace MissionFlow.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", schema: "identity");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value).HasColumnName("Email").IsRequired().HasMaxLength(200);
            email.HasIndex(e => e.Value).IsUnique();
        });
        builder.OwnsOne(u => u.Phone, phone =>
        {
            phone.Property(p => p.Number).HasColumnName("Phone").HasMaxLength(14);
        });
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(200);
        builder.Property(u => u.Role).IsRequired().HasConversion<string>().HasMaxLength(30);
        builder.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(u => u.Source).IsRequired().HasConversion<string>().HasMaxLength(30);
        builder.Property(u => u.ExternalId).HasMaxLength(100);
        builder.Property(u => u.PreferredLanguage).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.Property(u => u.LastLoginAt);
        builder.HasOne(u => u.Department).WithMany(d => d.Members).HasForeignKey(u => u.DepartmentId).OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(u => u.DepartmentId);
    }
}
