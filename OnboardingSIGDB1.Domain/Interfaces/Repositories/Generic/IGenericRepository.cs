namespace OnboardingSIGDB1.Domain.Interfaces.Repositories.Generic;

public interface IGenericRepository<T> : IDisposable where T : class
{
    Task Add(T entity);
    Task<T> GetById(int id);
    Task<List<T>> GetAll();
    void Update(T entity);
    void Remove(T entity);
}
