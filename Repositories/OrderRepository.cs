using Microsoft.EntityFrameworkCore;
using SampleApi.Data;
using SampleApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SampleApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Order> _dbSet;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Orders;
        }

        //Tüm Order kayıtlarını getirir
        public async Task<List<Order>> GetAllAsync() =>
            await _dbSet.Include(o => o.User)
                        .Include(o => o.Product)
                        .ToListAsync();

        //ID ye göre tek bir sipariş getirir
        public async Task<Order?> GetByIdAsync(int id) =>
            await _dbSet.Include(o => o.User)
                        .Include(o => o.Product)
                        .FirstOrDefaultAsync(o => o.Id == id);

        // Expression ile filtreleme: birden fazla kayıt döner
        public async Task<List<Order>> FindAsync(Expression<Func<Order, bool>> predicate) =>
            await _dbSet.Where(predicate)
                        .Include(o => o.User)
                        .Include(o => o.Product)
                        .ToListAsync();

        // Expression ile tek kayıt döner
        public async Task<Order?> FirstOrDefaultAsync(Expression<Func<Order, bool>> predicate) =>
            await _dbSet.Where(predicate)
                        .Include(o => o.User)
                        .Include(o => o.Product)
                        .FirstOrDefaultAsync();

        //Yeni sipariş ekler
        public async Task AddAsync(Order order)
        {
            await _dbSet.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        //Var olan siparişi günceller
        public async Task UpdateAsync(Order order)
        {
            _dbSet.Update(order);
            await _context.SaveChangesAsync();
        }

        //Siparişi siler
        public async Task DeleteAsync(Order order)
        {
            _dbSet.Remove(order);
            await _context.SaveChangesAsync();
        }

        //ID üzerinden sipariş siler
        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        //Birden fazla siparişi toplu ekler
        public async Task BulkInsertAsync(List<Order> orders)
        {
            await _dbSet.AddRangeAsync(orders);
            await _context.SaveChangesAsync();
        }

        //Kullanıcı ID'sine göre siparişleri getirir. Product ilişkisini de yükler
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId) =>
            await _dbSet.Where(o => o.UserId == userId)
                        .Include(o => o.Product)
                        .ToListAsync();

        //Kullanıcı ID'sine göre ürün detaylarıyla siparişleri getirir
        public async Task<List<Order>> GetOrdersWithProductsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(o => o.UserId == userId)
                .Include(o => o.Product)
                .ToListAsync();
        }
    }
}
