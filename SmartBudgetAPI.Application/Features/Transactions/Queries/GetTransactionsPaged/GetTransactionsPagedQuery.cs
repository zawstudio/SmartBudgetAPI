using MediatR;
using SmartBudgetAPI.Application.DTOs.Common;
using SmartBudgetAPI.Application.DTOs.Transactions;

namespace SmartBudgetAPI.Application.Features.Transactions.Queries.GetTransactionsPaged;

/// <summary>
/// Query to get paginated transactions
/// </summary>
public record GetTransactionsPagedQuery(
    Guid UserId,
    int PageNumber = 1,
    int PageSize = 10,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    Guid? CategoryId = null
) : IRequest<PagedResultDto<TransactionDto>>;

