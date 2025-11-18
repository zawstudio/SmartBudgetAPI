using MediatR;
using SmartBudgetAPI.Application.DTOs.Categories;
using SmartBudgetAPI.Domain.Interfaces;

namespace SmartBudgetAPI.Application.Features.Categories.Queries.GetAllCategories;

/// <summary>
/// Handler for GetAllCategoriesQuery
/// </summary>
public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.Categories.FindAsync(
            c => c.UserId == request.UserId && !c.IsDeleted,
            cancellationToken);

        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Color = c.Color,
            Icon = c.Icon,
            ParentCategoryId = c.ParentCategoryId,
            ParentCategoryName = c.ParentCategory?.Name,
            CreatedAt = c.CreatedAt
        }).ToList();
    }
}

