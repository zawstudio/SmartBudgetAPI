using System.Text.Json;
using MediatR;
using SmartBudgetAPI.Application.DTOs.Common;
using SmartBudgetAPI.Application.DTOs.Transactions;
using SmartBudgetAPI.Domain.Interfaces;

namespace SmartBudgetAPI.Application.Features.Transactions.Queries.GetTransactionsPaged;

/// <summary>
/// Handler for GetTransactionsPagedQuery
/// </summary>
public class GetTransactionsPagedQueryHandler : IRequestHandler<GetTransactionsPagedQuery, PagedResultDto<TransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTransactionsPagedQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResultDto<TransactionDto>> Handle(GetTransactionsPagedQuery request, CancellationToken cancellationToken)
    {
        var (transactions, totalCount) = await _unitOfWork.Transactions.GetFilteredPagedAsync(
            request.UserId,
            request.StartDate,
            request.EndDate,
            request.CategoryId,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var transactionDtos = transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Amount = t.Amount,
            Currency = t.Currency,
            Type = t.Type,
            TypeName = t.Type.ToString(),
            TransactionDate = t.TransactionDate,
            CategoryId = t.CategoryId,
            CategoryName = t.Category?.Name ?? "Unknown",
            CategoryColor = t.Category?.Color,
            Tags = !string.IsNullOrEmpty(t.Tags) ? JsonSerializer.Deserialize<List<string>>(t.Tags) : null,
            Location = t.Location,
            ReceiptUrl = t.ReceiptUrl,
            Notes = t.Notes,
            CreatedAt = t.CreatedAt
        }).ToList();

        return new PagedResultDto<TransactionDto>
        {
            Items = transactionDtos,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages,
            TotalCount = totalCount
        };
    }
}

