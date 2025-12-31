using SampleApi.Models.DTOs;
using SampleApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IContactService
{
    Task<List<Contact>> GetAllAsync();
    Task<Contact?> GetByIdAsync(int id);
    Task AddAsync(Contact contact);
    Task<bool> UpdateAsync(int id, UpdateContactDto dto);
    Task DeleteAsync(int id);
}
