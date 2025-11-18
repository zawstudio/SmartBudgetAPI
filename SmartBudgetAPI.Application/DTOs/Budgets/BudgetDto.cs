using SmartBudgetAPI.Domain.Enums;

namespace SmartBudgetAPI.Application.DTOs.Budgets;

/// <summary>
/// Budget data transfer object
/// </summary>
public class BudgetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal LimitAmount { get; set; }
    public string Currency { get; set; } = "PLN";
    public BudgetPeriod Period { get; set; }
    public string PeriodName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public decimal ThresholdPercentage { get; set; }
    public bool IsActive { get; set; }
    public decimal CurrentSpending { get; set; }
    public decimal RemainingAmount { get; set; }
    public decimal PercentageUsed { get; set; }
    public DateTime CreatedAt { get; set; }
}

