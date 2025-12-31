using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SampleApi.Repositories
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAllAsync();
        Task<Address?> GetByIdAsync(int id);

        // Expression ile filtreleme: birden fazla kayıt döner
        Task<List<Address>> FindAsync(Expression<Func<Address, bool>> predicate);

        // Expression ile tek kayıt döner
        Task<Address?> FirstOrDefaultAsync(Expression<Func<Address, bool>> predicate);

        Task AddAsync(Address entity);
        Task UpdateAsync(Address entity);
        Task DeleteAsync(Address entity);
        Task DeleteAsync(int id);
    }
}
