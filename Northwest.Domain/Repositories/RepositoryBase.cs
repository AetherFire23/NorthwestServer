using Microsoft.EntityFrameworkCore;
using Northwest.Persistence.Entities;

namespace Northwest.Domain.Repositories;

public abstract class RepositoryBase<TEntity, TContext>
    where TEntity : class, IEntity
    where TContext : DbContext

{
    protected readonly TContext Context;
    protected DbSet<TEntity> Set => Context.Set<TEntity>();

    public RepositoryBase(TContext context)
    {
        Context = context;
    }

    protected async Task<TEntity> GetById(Guid id)
    {
        TEntity entity = await Set.FirstAsync(x => x.Id == id);
        return entity;
    }

    protected async Task<TEntity> TaskGetByIdOrDefault(Guid id)
    {
        TEntity entity = await Set.FirstAsync(x => x.Id == id);
        return entity;
    }

    protected async Task AddEntity(TEntity entity)
    {
        _ = await Set.AddAsync(entity);
    }

    protected void DeleteEntity(TEntity entity)
    {
        _ = Set.Remove(entity);
    }
}
