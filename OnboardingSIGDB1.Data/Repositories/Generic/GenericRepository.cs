using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Domain.Interfaces.Repositories.Generic;

namespace OnboardingSIGDB1.Data.Repositories.Generic;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly DataContext Db;
    protected readonly DbSet<T> DbSet;

    protected GenericRepository(DataContext context)
    {
        Db = context;
        DbSet = Db.Set<T>();
    }

    public async Task Add(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public virtual async Task<T> GetById(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<List<T>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}