using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SampleApi.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);

    //Expression ile filtreleme 
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

    //Expression ile tek kayıt 
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
