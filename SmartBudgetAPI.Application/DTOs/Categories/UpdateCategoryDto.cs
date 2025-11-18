namespace SmartBudgetAPI.Application.DTOs.Categories;

/// <summary>
/// DTO for updating a category
/// </summary>
public class UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#000000";
    public string Icon { get; set; } = "default";
    public Guid? ParentCategoryId { get; set; }
}

