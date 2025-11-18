using System.Text.Json;
using MediatR;
using SmartBudgetAPI.Application.DTOs.Transactions;
using SmartBudgetAPI.Domain.Entities;
using SmartBudgetAPI.Domain.Interfaces;

namespace SmartBudgetAPI.Application.Features.Transactions.Commands.CreateTransaction;

/// <summary>
/// Handler for CreateTransactionCommand
/// </summary>
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.TransactionDto;

        var category = await _unitOfWork.Categories.FirstOrDefaultAsync(
            c => c.Id == dto.CategoryId && c.UserId == request.UserId,
            cancellationToken);

        if (category == null)
        {
            throw new InvalidOperationException("Category not found");
        }

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Type = dto.Type,
            TransactionDate = dto.TransactionDate,
            UserId = request.UserId,
            CategoryId = dto.CategoryId,
            Tags = dto.Tags != null ? JsonSerializer.Serialize(dto.Tags) : null,
            Location = dto.Location,
            ReceiptUrl = dto.ReceiptUrl,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Transactions.AddAsync(transaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TransactionDto
        {
            Id = transaction.Id,
            Title = transaction.Title,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            Type = transaction.Type,
            TypeName = transaction.Type.ToString(),
            TransactionDate = transaction.TransactionDate,
            CategoryId = transaction.CategoryId,
            CategoryName = category.Name,
            CategoryColor = category.Color,
            Tags = dto.Tags,
            Location = transaction.Location,
            ReceiptUrl = transaction.ReceiptUrl,
            Notes = transaction.Notes,
            CreatedAt = transaction.CreatedAt
        };
    }
}

