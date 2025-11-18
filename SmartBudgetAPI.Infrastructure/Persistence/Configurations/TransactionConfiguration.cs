using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBudgetAPI.Domain.Entities;

namespace SmartBudgetAPI.Infrastructure.Persistence.Configurations;

/// <summary>
/// Transaction entity configuration
/// </summary>
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(t => t.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(t => t.Type)
            .IsRequired();

        builder.Property(t => t.TransactionDate)
            .IsRequired();

        builder.Property(t => t.Tags)
            .HasMaxLength(500);

        builder.Property(t => t.Location)
            .HasMaxLength(200);

        builder.Property(t => t.ReceiptUrl)
            .HasMaxLength(500);

        builder.Property(t => t.Notes)
            .HasMaxLength(2000);

        builder.HasIndex(t => t.TransactionDate);
        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.CategoryId);

        builder.HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

