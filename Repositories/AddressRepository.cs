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
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Address> _dbSet;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Addresses;
        }

        // Tüm kayıtları getir
        public async Task<List<Address>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        // ID ile tek kayıt getir
        public async Task<Address?> GetByIdAsync(int id) =>
            await _dbSet.FirstOrDefaultAsync(a => a.Id == id);

        // Expression ile birden fazla kayıt bul
        public async Task<List<Address>> FindAsync(Expression<Func<Address, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        // Expression ile tek kayıt bul
        public async Task<Address?> FirstOrDefaultAsync(Expression<Func<Address, bool>> predicate) =>
            await _dbSet.FirstOrDefaultAsync(predicate);

        // Yeni kayıt ekle
        public async Task AddAsync(Address entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Kayıt güncelle
        public async Task UpdateAsync(Address entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Address nesnesi ile sil
        public async Task DeleteAsync(Address entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // ID ile sil
        public async Task DeleteAsync(int id)
        {
            var address = await GetByIdAsync(id);
            if (address != null)
            {
                await DeleteAsync(address);
            }
        }
    }
}
