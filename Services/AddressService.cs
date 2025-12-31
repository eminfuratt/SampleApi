using SampleApi.Models;
using SampleApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;

        public AddressService(IAddressRepository repository) => _repository = repository;

        // Tüm adresleri getirir repository'den
        public Task<List<Address>> GetAllAsync() => _repository.GetAllAsync();

        //ID ye göre adres getirir
        public Task<Address?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        //Yeni adres ekler 
        public Task AddAsync(Address address) => _repository.AddAsync(address);

        //Adres günceller 
        public Task UpdateAsync(Address address) => _repository.UpdateAsync(address);

        //Adres siler
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
        
    }
}
