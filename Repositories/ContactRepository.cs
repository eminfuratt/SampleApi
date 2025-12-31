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
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Contact> _dbSet;

        public ContactRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Contacts;
        }

        // Tüm kayıtları getir
        public async Task<List<Contact>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        // ID ile tek kayıt getir
        public async Task<Contact?> GetByIdAsync(int id) =>
            await _dbSet.FirstOrDefaultAsync(c => c.Id == id);

        // Expression ile birden fazla kayıt bul
        public async Task<List<Contact>> FindAsync(Expression<Func<Contact, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        // Expression ile tek kayıt bul
        public async Task<Contact?> FirstOrDefaultAsync(Expression<Func<Contact, bool>> predicate) =>
            await _dbSet.FirstOrDefaultAsync(predicate);

        // Yeni kayıt ekle
        public async Task AddAsync(Contact entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Kayıt güncelle
        public async Task UpdateAsync(Contact entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Contact nesnesi ile sil
        public async Task DeleteAsync(Contact entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // ID ile sil
        public async Task DeleteAsync(int id)
        {
            var contact = await GetByIdAsync(id);
            if (contact != null)
            {
                await DeleteAsync(contact);
            }
        }
    }
}
