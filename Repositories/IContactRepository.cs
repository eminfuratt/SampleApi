using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SampleApi.Repositories
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllAsync();
        Task<Contact?> GetByIdAsync(int id);
        Task<List<Contact>> FindAsync(Expression<Func<Contact, bool>> predicate);
        Task<Contact?> FirstOrDefaultAsync(Expression<Func<Contact, bool>> predicate);
        Task AddAsync(Contact contact);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(Contact contact);
        Task DeleteAsync(int id);
    }
}
