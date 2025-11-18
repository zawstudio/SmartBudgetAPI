using FluentValidation;

namespace SmartBudgetAPI.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Validator for CreateCategoryCommand
/// </summary>
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryDto.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters");

        RuleFor(x => x.CategoryDto.Color)
            .NotEmpty().WithMessage("Color is required")
            .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$").WithMessage("Invalid color format. Use hex format (#FFFFFF)");

        RuleFor(x => x.CategoryDto.Icon)
            .NotEmpty().WithMessage("Icon is required")
            .MaximumLength(50).WithMessage("Icon must not exceed 50 characters");
    }
}

