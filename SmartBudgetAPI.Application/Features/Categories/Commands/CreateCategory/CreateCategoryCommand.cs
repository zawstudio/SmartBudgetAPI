using MediatR;
using SmartBudgetAPI.Application.DTOs.Categories;

namespace SmartBudgetAPI.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Command for creating a category
/// </summary>
public record CreateCategoryCommand(Guid UserId, CreateCategoryDto CategoryDto) : IRequest<CategoryDto>;

