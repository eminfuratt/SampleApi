using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SampleApi.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> FindAsync(Expression<Func<User, bool>> predicate);
        Task<User?> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate);
        Task AddAsync(User entity);
        Task UpdateAsync(User entity);
        Task DeleteAsync(int id);
        Task<User?> GetByEmailAsync(string email);
    }
}
