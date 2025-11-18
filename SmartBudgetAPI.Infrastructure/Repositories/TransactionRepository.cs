using Microsoft.EntityFrameworkCore;
using SmartBudgetAPI.Domain.Entities;
using SmartBudgetAPI.Domain.Enums;
using SmartBudgetAPI.Domain.Interfaces;
using SmartBudgetAPI.Infrastructure.Persistence;

namespace SmartBudgetAPI.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for transactions with additional methods
/// </summary>
public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.CategoryId == categoryId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.UserId == userId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByTypeAsync(Guid userId, TransactionType type, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.UserId == userId && t.Type == type)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalAmountAsync(Guid userId, DateTime startDate, DateTime endDate, TransactionType? type = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(t => t.UserId == userId && t.TransactionDate >= startDate && t.TransactionDate <= endDate);

        if (type.HasValue)
        {
            query = query.Where(t => t.Type == type.Value);
        }

        return await query.SumAsync(t => t.Amount, cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetPagedAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.TransactionDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<Guid, decimal>> GetSummaryCategoriesAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.UserId == userId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
            .GroupBy(t => t.CategoryId)
            .Select(g => new { CategoryId = g.Key, Total = g.Sum(t => t.Amount) })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Total, cancellationToken);
    }
}

