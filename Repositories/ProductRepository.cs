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
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Product> _dbSet;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Products;
        }

        //Tüm ürünleri getirir
        public async Task<List<Product>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        // ID'ye göre tek ürün getirir
        public async Task<Product?> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id);

        // Expression ile filtreleme: birden fazla kayıt döner
        public async Task<List<Product>> FindAsync(Expression<Func<Product, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        // Expression ile tek kayıt döner
        public async Task<Product?> FirstOrDefaultAsync(Expression<Func<Product, bool>> predicate) =>
            await _dbSet.FirstOrDefaultAsync(predicate);

        // Yeni ürün ekler
        public async Task AddAsync(Product entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        //Var olan ürünü günceller
        public async Task UpdateAsync(Product entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        //Ürünü siler
        public async Task DeleteAsync(Product entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
