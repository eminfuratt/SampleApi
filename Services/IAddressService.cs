using SampleApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Services
{
    public interface IAddressService
    {
        Task<List<Address>> GetAllAsync();
        Task<Address?> GetByIdAsync(int id);
        Task AddAsync(Address address);
        Task UpdateAsync(Address address);
        Task DeleteAsync(int id);
    }
}
