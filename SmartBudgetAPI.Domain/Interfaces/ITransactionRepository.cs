using SmartBudgetAPI.Domain.Entities;
using SmartBudgetAPI.Domain.Enums;

namespace SmartBudgetAPI.Domain.Interfaces;

/// <summary>
/// Extended repository interface for transactions
/// </summary>
public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByTypeAsync(Guid userId, TransactionType type, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalAmountAsync(Guid userId, DateTime startDate, DateTime endDate, TransactionType? type = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetPagedAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, decimal>> GetSummaryCategoriesAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}

