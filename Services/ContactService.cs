using SampleApi.Models;
using SampleApi.Models.DTOs;
using SampleApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Contact>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Contact?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Contact contact) => _repository.AddAsync(contact);

        public Task UpdateAsync(Contact contact) => _repository.UpdateAsync(contact);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);

        public async Task<bool> UpdateAsync(int id, UpdateContactDto dto)
        {
            var contact = await _repository.GetByIdAsync(id);
            if (contact == null)
                return false;

            contact.Type = dto.Type;
            contact.Value = dto.Value;

            await _repository.UpdateAsync(contact);
            return true;
        }
    }
}
