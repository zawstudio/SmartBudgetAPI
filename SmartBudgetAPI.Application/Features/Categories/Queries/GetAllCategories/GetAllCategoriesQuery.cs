using MediatR;
using SmartBudgetAPI.Application.DTOs.Categories;

namespace SmartBudgetAPI.Application.Features.Categories.Queries.GetAllCategories;

/// <summary>
/// Query to get all user categories
/// </summary>
public record GetAllCategoriesQuery(Guid UserId) : IRequest<List<CategoryDto>>;

