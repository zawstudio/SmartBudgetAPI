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
        var allTransactions = await _unitOfWork.Transactions.FindAsync(
            t => t.UserId == request.UserId && !t.IsDeleted,
            cancellationToken);

        var filtered = allTransactions.AsQueryable();

        if (request.StartDate.HasValue)
        {
            filtered = filtered.Where(t => t.TransactionDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            filtered = filtered.Where(t => t.TransactionDate <= request.EndDate.Value);
        }

        if (request.CategoryId.HasValue)
        {
            filtered = filtered.Where(t => t.CategoryId == request.CategoryId.Value);
        }

        var totalCount = filtered.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var transactions = filtered
            .OrderByDescending(t => t.TransactionDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

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

