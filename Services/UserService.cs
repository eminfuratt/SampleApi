using SampleApi.Models;
using SampleApi.Models.DTOs;
using SampleApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public Task<List<User>> GetAllAsync() => _repository.GetAllAsync();

        public Task<User?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(User user) => _repository.AddAsync(user);

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return false;

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Role = dto.Role;

            await _repository.UpdateAsync(user);
            return true;
        }

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}
