namespace SmartBudgetAPI.Application.DTOs.Categories;

/// <summary>
/// Category data transfer object
/// </summary>
public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#000000";
    public string Icon { get; set; } = "default";
    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CategoryDto> SubCategories { get; set; } = new();
}

