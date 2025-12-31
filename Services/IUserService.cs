using SampleApi.Models;
using SampleApi.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<bool> UpdateAsync(int id, UpdateUserDto dto);
        Task DeleteAsync(int id);
    }
}
