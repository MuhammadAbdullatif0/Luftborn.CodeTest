using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly StoreDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(StoreDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity);
        if (!_repositories.TryGetValue(type, out var repo))
        {
            var repoInstance = new GenericRepository<TEntity>(_context);
            _repositories.Add(type, repoInstance);
            return repoInstance;
        }
        return (IGenericRepository<TEntity>)repo;
    }

    public async Task<bool> Complete() => await _context.SaveChangesAsync() > 0;

    public void Dispose() => _context.Dispose();
}
