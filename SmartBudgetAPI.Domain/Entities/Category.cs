using SmartBudgetAPI.Domain.Common;

namespace SmartBudgetAPI.Domain.Entities;

/// <summary>
/// Transaction category entity
/// </summary>
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#000000";
    public string Icon { get; set; } = "default";
    public Guid UserId { get; set; }
    public Guid? ParentCategoryId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}

