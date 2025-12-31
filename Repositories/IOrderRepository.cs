using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SampleApi.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);

        // Expression ile filtreleme
        Task<List<Order>> FindAsync(Expression<Func<Order, bool>> predicate);
        Task<Order?> FirstOrDefaultAsync(Expression<Func<Order, bool>> predicate);

        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
        Task DeleteAsync(int id);
        Task BulkInsertAsync(List<Order> orders);

        //userId'ye ait orderları product include ile döner
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<List<Order>> GetOrdersWithProductsByUserIdAsync(int userId);

    }
}