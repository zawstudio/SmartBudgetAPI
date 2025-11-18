using SmartBudgetAPI.Domain.Common;

namespace SmartBudgetAPI.Domain.Entities;

/// <summary>
/// User entity
/// </summary>
public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string DefaultCurrency { get; set; } = "USD";
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public virtual ICollection<BudgetAlert> BudgetAlerts { get; set; } = new List<BudgetAlert>();

    public string FullName => $"{FirstName} {LastName}";
}

