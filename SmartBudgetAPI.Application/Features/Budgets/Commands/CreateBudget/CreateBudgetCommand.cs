using MediatR;
using SmartBudgetAPI.Application.DTOs.Budgets;

namespace SmartBudgetAPI.Application.Features.Budgets.Commands.CreateBudget;

/// <summary>
/// Command for creating a budget
/// </summary>
public record CreateBudgetCommand(Guid UserId, CreateBudgetDto BudgetDto) : IRequest<BudgetDto>;

