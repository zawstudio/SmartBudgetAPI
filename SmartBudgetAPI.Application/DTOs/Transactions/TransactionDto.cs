using SmartBudgetAPI.Domain.Enums;

namespace SmartBudgetAPI.Application.DTOs.Transactions;

/// <summary>
/// Transaction data transfer object
/// </summary>
public class TransactionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "PLN";
    public TransactionType Type { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? CategoryColor { get; set; }
    public List<string>? Tags { get; set; }
    public string? Location { get; set; }
    public string? ReceiptUrl { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

