using FluentValidation;

namespace SmartBudgetAPI.Application.Features.Transactions.Commands.CreateTransaction;

/// <summary>
/// Validator for CreateTransactionCommand
/// </summary>
public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.TransactionDto.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.TransactionDto.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(x => x.TransactionDto.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be 3 characters (ISO code)");

        RuleFor(x => x.TransactionDto.Type)
            .IsInEnum().WithMessage("Invalid transaction type");

        RuleFor(x => x.TransactionDto.TransactionDate)
            .NotEmpty().WithMessage("Transaction date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transaction date cannot be in the future");

        RuleFor(x => x.TransactionDto.CategoryId)
            .NotEmpty().WithMessage("Category is required");
    }
}

