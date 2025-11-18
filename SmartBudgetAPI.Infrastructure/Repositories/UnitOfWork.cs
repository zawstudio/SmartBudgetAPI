using Microsoft.EntityFrameworkCore.Storage;
using SmartBudgetAPI.Domain.Entities;
using SmartBudgetAPI.Domain.Interfaces;
using SmartBudgetAPI.Infrastructure.Persistence;

namespace SmartBudgetAPI.Infrastructure.Repositories;

/// <summary>
/// Unit of Work Pattern implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public IGenericRepository<User> Users { get; }
    public IGenericRepository<Category> Categories { get; }
    public ITransactionRepository Transactions { get; }
    public IGenericRepository<Budget> Budgets { get; }
    public IGenericRepository<BudgetAlert> BudgetAlerts { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new GenericRepository<User>(_context);
        Categories = new GenericRepository<Category>(_context);
        Transactions = new TransactionRepository(_context);
        Budgets = new GenericRepository<Budget>(_context);
        BudgetAlerts = new GenericRepository<BudgetAlert>(_context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

