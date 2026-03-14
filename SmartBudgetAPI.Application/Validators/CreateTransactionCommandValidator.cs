using FluentValidation;
using SmartBudgetAPI.Application.Features.Transactions.Commands.CreateTransaction;

namespace SmartBudgetAPI.Application.Validators;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(v => v.TransactionDto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(v => v.TransactionDto.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(v => v.TransactionDto.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.");

        RuleFor(v => v.TransactionDto.TransactionDate)
            .NotEmpty().WithMessage("Transaction date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transaction date cannot be in the future.");

        RuleFor(v => v.TransactionDto.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.");
    }
}
