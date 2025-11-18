using SmartBudgetAPI.Domain.Enums;

namespace SmartBudgetAPI.Application.DTOs.Budgets;

/// <summary>
/// DTO for creating a budget
/// </summary>
public class CreateBudgetDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal LimitAmount { get; set; }
    public string Currency { get; set; } = "PLN";
    public BudgetPeriod Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal ThresholdPercentage { get; set; } = 80;
}

