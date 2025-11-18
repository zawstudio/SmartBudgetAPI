using MediatR;
using SmartBudgetAPI.Application.DTOs.Categories;
using SmartBudgetAPI.Domain.Entities;
using SmartBudgetAPI.Domain.Interfaces;

namespace SmartBudgetAPI.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Handler for CreateCategoryCommand
/// </summary>
public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CategoryDto;

        if (dto.ParentCategoryId.HasValue)
        {
            var parentExists = await _unitOfWork.Categories.AnyAsync(
                c => c.Id == dto.ParentCategoryId.Value && c.UserId == request.UserId,
                cancellationToken);

            if (!parentExists)
            {
                throw new InvalidOperationException("Parent category not found");
            }
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Color = dto.Color,
            Icon = dto.Icon,
            UserId = request.UserId,
            ParentCategoryId = dto.ParentCategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Categories.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.Color,
            Icon = category.Icon,
            ParentCategoryId = category.ParentCategoryId,
            CreatedAt = category.CreatedAt
        };
    }
}

