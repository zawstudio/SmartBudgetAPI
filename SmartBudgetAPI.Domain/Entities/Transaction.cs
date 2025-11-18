using SmartBudgetAPI.Domain.Common;
using SmartBudgetAPI.Domain.Enums;
using SmartBudgetAPI.Domain.ValueObjects;

namespace SmartBudgetAPI.Domain.Entities;

/// <summary>
/// Financial transaction entity
/// </summary>
public class Transaction : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public TransactionType Type { get; set; }
    public DateTime TransactionDate { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string? Tags { get; set; }
    public string? Location { get; set; }
    public string? ReceiptUrl { get; set; }
    public string? Notes { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;

    public Money MoneyAmount => new Money(Amount, Currency);
}

