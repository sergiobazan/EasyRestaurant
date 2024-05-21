using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

internal abstract class Repository<TEntity> where TEntity : Entity
{
    protected readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public virtual async Task<TEntity?> GetAsync(Guid id)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(c => c.Id == id);
    }
}
