using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MissionFlow.Domain.Entities;

namespace MissionFlow.Infrastructure.Persistence.Configurations;

public sealed class MissionConfiguration : IEntityTypeConfiguration<Mission>
{
    public void Configure(EntityTypeBuilder<Mission> builder)
    {
        builder.ToTable("Missions", schema: "operations");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.MissionNumber).IsRequired().HasMaxLength(30);
        builder.HasIndex(m => m.MissionNumber).IsUnique();
        builder.Property(m => m.Title).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Description).HasMaxLength(1000);
        builder.Property(m => m.Type).IsRequired().HasConversion<string>().HasMaxLength(30);
        builder.Property(m => m.Status).IsRequired().HasConversion<string>().HasMaxLength(30);
        builder.OwnsOne(m => m.Period, period =>
        {
            period.Property(p => p.StartDate).HasColumnName("StartDate").IsRequired();
            period.Property(p => p.EndDate).HasColumnName("EndDate").IsRequired();
        });
        builder.OwnsOne(m => m.DestinationAddress, addr =>
        {
            addr.Property(a => a.Street).HasColumnName("DestinationStreet").HasMaxLength(200);
            addr.Property(a => a.Complement).HasColumnName("DestinationComplement").HasMaxLength(200);
            addr.Property(a => a.City).HasColumnName("DestinationCity").HasMaxLength(100);
            addr.Property(a => a.Wilaya).HasColumnName("DestinationWilaya").HasMaxLength(100);
            addr.Property(a => a.PostalCode).HasColumnName("DestinationPostalCode").HasMaxLength(10);
            addr.Property(a => a.Country).HasColumnName("DestinationCountry").HasMaxLength(50);
        });
        builder.Property(m => m.Objective).HasMaxLength(500);
        builder.OwnsOne(m => m.EstimatedBudget, money =>
        {
            money.Property(mb => mb.Amount).HasColumnName("EstimatedBudgetAmount").HasColumnType("decimal(18,2)");
            money.Property(mb => mb.Currency).HasColumnName("EstimatedBudgetCurrency").HasMaxLength(3);
        });
        builder.OwnsOne(m => m.TotalExpenses, money =>
        {
            money.Property(mb => mb.Amount).HasColumnName("TotalExpensesAmount").HasColumnType("decimal(18,2)");
            money.Property(mb => mb.Currency).HasColumnName("TotalExpensesCurrency").HasMaxLength(3);
        });
        builder.Property(m => m.TransportMode).IsRequired().HasConversion<string>().HasMaxLength(30);
        builder.HasOne(m => m.Requester).WithMany().HasForeignKey(m => m.RequesterId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Vehicle).WithMany().HasForeignKey(m => m.VehicleId).OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(m => m.RequesterId);
        builder.HasIndex(m => m.Status);
        builder.HasIndex(m => m.Type);
    }
}
