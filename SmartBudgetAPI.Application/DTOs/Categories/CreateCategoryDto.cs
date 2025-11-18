namespace SmartBudgetAPI.Application.DTOs.Categories;

/// <summary>
/// DTO for creating a category
/// </summary>
public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#000000";
    public string Icon { get; set; } = "default";
    public Guid? ParentCategoryId { get; set; }
}

