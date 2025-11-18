using MediatR;
using SmartBudgetAPI.Application.DTOs.Transactions;

namespace SmartBudgetAPI.Application.Features.Transactions.Commands.CreateTransaction;

/// <summary>
/// Command for creating a transaction
/// </summary>
public record CreateTransactionCommand(Guid UserId, CreateTransactionDto TransactionDto) : IRequest<TransactionDto>;

