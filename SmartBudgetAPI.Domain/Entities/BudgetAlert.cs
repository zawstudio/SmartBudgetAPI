using SmartBudgetAPI.Domain.Common;
using SmartBudgetAPI.Domain.Enums;

namespace SmartBudgetAPI.Domain.Entities;

/// <summary>
/// Budget alert entity
/// </summary>
public class BudgetAlert : BaseEntity
{
    public Guid BudgetId { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public AlertStatus Status { get; set; }
    public decimal CurrentSpending { get; set; }
    public decimal BudgetLimit { get; set; }
    public decimal PercentageUsed { get; set; }
    public DateTime TriggeredAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public virtual Budget Budget { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

