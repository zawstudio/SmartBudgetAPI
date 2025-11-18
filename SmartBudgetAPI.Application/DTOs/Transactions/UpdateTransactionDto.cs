using SmartBudgetAPI.Domain.Enums;

namespace SmartBudgetAPI.Application.DTOs.Transactions;

/// <summary>
/// DTO for updating a transaction
/// </summary>
public class UpdateTransactionDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "PLN";
    public TransactionType Type { get; set; }
    public DateTime TransactionDate { get; set; }
    public Guid CategoryId { get; set; }
    public List<string>? Tags { get; set; }
    public string? Location { get; set; }
    public string? ReceiptUrl { get; set; }
    public string? Notes { get; set; }
}

