using SmartBudgetAPI.Domain.Common;
using SmartBudgetAPI.Domain.Enums;
using SmartBudgetAPI.Domain.ValueObjects;

namespace SmartBudgetAPI.Domain.Entities;

/// <summary>
/// Budget entity
/// </summary>
public class Budget : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal LimitAmount { get; set; }
    public string Currency { get; set; } = "PLN";
    public BudgetPeriod Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid UserId { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal ThresholdPercentage { get; set; } = 80;
    public bool IsActive { get; set; } = true;

    public virtual User User { get; set; } = null!;
    public virtual Category? Category { get; set; }
    public virtual ICollection<BudgetAlert> BudgetAlerts { get; set; } = new List<BudgetAlert>();

    public Money LimitMoney => new Money(LimitAmount, Currency);
    public DateRange DateRange => new DateRange(StartDate, EndDate);
}

