using MediatR;
using SmartBudgetAPI.Application.DTOs.Budgets;
using SmartBudgetAPI.Domain.Entities;
using SmartBudgetAPI.Domain.Enums;
using SmartBudgetAPI.Domain.Interfaces;

namespace SmartBudgetAPI.Application.Features.Budgets.Commands.CreateBudget;

/// <summary>
/// Handler for CreateBudgetCommand
/// </summary>
public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, BudgetDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBudgetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BudgetDto> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var dto = request.BudgetDto;

        if (dto.CategoryId.HasValue)
        {
            var categoryExists = await _unitOfWork.Categories.AnyAsync(
                c => c.Id == dto.CategoryId.Value && c.UserId == request.UserId,
                cancellationToken);

            if (!categoryExists)
            {
                throw new InvalidOperationException("Category not found");
            }
        }

        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            LimitAmount = dto.LimitAmount,
            Currency = dto.Currency,
            Period = dto.Period,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            UserId = request.UserId,
            CategoryId = dto.CategoryId,
            ThresholdPercentage = dto.ThresholdPercentage,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Budgets.AddAsync(budget, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var currentSpending = await CalculateCurrentSpending(budget, cancellationToken);

        return new BudgetDto
        {
            Id = budget.Id,
            Name = budget.Name,
            Description = budget.Description,
            LimitAmount = budget.LimitAmount,
            Currency = budget.Currency,
            Period = budget.Period,
            PeriodName = budget.Period.ToString(),
            StartDate = budget.StartDate,
            EndDate = budget.EndDate,
            CategoryId = budget.CategoryId,
            ThresholdPercentage = budget.ThresholdPercentage,
            IsActive = budget.IsActive,
            CurrentSpending = currentSpending,
            RemainingAmount = budget.LimitAmount - currentSpending,
            PercentageUsed = budget.LimitAmount > 0 ? (currentSpending / budget.LimitAmount * 100) : 0,
            CreatedAt = budget.CreatedAt
        };
    }

    private async Task<decimal> CalculateCurrentSpending(Budget budget, CancellationToken cancellationToken)
    {
        var spending = await _unitOfWork.Transactions.GetTotalAmountAsync(
            budget.UserId,
            budget.StartDate,
            budget.EndDate,
            TransactionType.Expense,
            cancellationToken);

        return spending;
    }
}

