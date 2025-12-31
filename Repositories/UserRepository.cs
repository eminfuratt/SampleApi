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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Users;
        }

        // Tüm kullanıcıları getirir
        public async Task<List<User>> GetAllAsync() =>
            await _dbSet.ToListAsync();
        
        // ID'ye göre tek kullanıcı getirir
        public async Task<User?> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id);

        // Expression ile filtreleme: birden fazla kayıt döner
        public async Task<List<User>> FindAsync(Expression<Func<User, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        // Expression ile tek kayıt döner
        public async Task<User?> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate) =>
            await _dbSet.FirstOrDefaultAsync(predicate);

        // Yeni kullanıcı ekler
        public async Task AddAsync(User entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Var olan kullanıcıyı günceller
        public async Task UpdateAsync(User entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        
        // ID ile kullanıcıyı siler
        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _dbSet.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        // Email ile kullanıcı getirir (login veya kontrol için kullanılır)
        public async Task<User?> GetByEmailAsync(string email) =>
            await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }
}
