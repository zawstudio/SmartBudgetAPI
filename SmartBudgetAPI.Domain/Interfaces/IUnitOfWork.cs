using SmartBudgetAPI.Domain.Entities;

namespace SmartBudgetAPI.Domain.Interfaces;

/// <summary>
/// Unit of Work Pattern - manages database transactions
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<User> Users { get; }
    IGenericRepository<Category> Categories { get; }
    ITransactionRepository Transactions { get; }
    IGenericRepository<Budget> Budgets { get; }
    IGenericRepository<BudgetAlert> BudgetAlerts { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

