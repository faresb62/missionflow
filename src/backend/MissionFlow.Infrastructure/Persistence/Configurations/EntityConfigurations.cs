using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MissionFlow.Domain.Entities;

namespace MissionFlow.Infrastructure.Persistence.Configurations;

public sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expenses", schema: "operations");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
        builder.OwnsOne(e => e.Amount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)").IsRequired();
            money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
        });
        builder.Property(e => e.Category).IsRequired().HasConversion<string>().HasMaxLength(30);
        builder.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(30);
        builder.Property(e => e.ExpenseDate).IsRequired();
        builder.Property(e => e.ReceiptUrl).HasMaxLength(500);
        builder.Property(e => e.Notes).HasMaxLength(1000);
        builder.Property(e => e.HasReceipt).IsRequired().HasDefaultValue(false);
        builder.HasOne(e => e.SubmittedBy).WithMany().HasForeignKey(e => e.SubmittedById).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(e => e.MissionId);
        builder.HasIndex(e => e.SubmittedById);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.Category);
    }
}

public sealed class ApprovalConfiguration : IEntityTypeConfiguration<Approval>
{
    public void Configure(EntityTypeBuilder<Approval> builder)
    {
        builder.ToTable("Approvals", schema: "operations");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Decision).IsRequired().HasConversion<string>().HasMaxLength(30);
        builder.Property(a => a.Comment).HasMaxLength(1000);
        builder.Property(a => a.DecidedAt).IsRequired();
        builder.HasOne(a => a.Approver).WithMany().HasForeignKey(a => a.ApproverId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(a => a.MissionId);
        builder.HasIndex(a => a.ApproverId);
    }
}

public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles", schema: "operations");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Brand).IsRequired().HasMaxLength(100);
        builder.Property(v => v.Model).IsRequired().HasMaxLength(100);
        builder.Property(v => v.RegistrationNumber).IsRequired().HasMaxLength(30);
        builder.HasIndex(v => v.RegistrationNumber).IsUnique();
        builder.Property(v => v.Type).IsRequired().HasConversion<string>().HasMaxLength(30);
        builder.Property(v => v.Year);
        builder.Property(v => v.IsAvailable).HasDefaultValue(true);
        builder.Property(v => v.MileageKm).HasColumnType("decimal(10,1)");
        builder.Property(v => v.Notes).HasMaxLength(500);
    }
}

public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments", schema: "organization");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
        builder.Property(d => d.Code).IsRequired().HasMaxLength(20);
        builder.HasIndex(d => d.Code).IsUnique();
        builder.Property(d => d.Description).HasMaxLength(500);
        builder.Property(d => d.IsActive).HasDefaultValue(true);
        builder.HasOne(d => d.ParentDepartment).WithMany(d => d.ChildDepartments).HasForeignKey(d => d.ParentDepartmentId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens", schema: "identity");
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.TokenHash).IsRequired().HasMaxLength(512);
        builder.HasIndex(rt => rt.TokenHash).IsUnique();
        builder.Property(rt => rt.ExpiresAt).IsRequired();
        builder.Property(rt => rt.IsRevoked).IsRequired().HasDefaultValue(false);
        builder.Property(rt => rt.RevokedAt);
        builder.Property(rt => rt.RevokedReason).HasMaxLength(500);
        builder.HasOne(rt => rt.User).WithMany().HasForeignKey(rt => rt.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(rt => rt.UserId);
    }
}
