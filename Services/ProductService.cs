using SampleApi.Models;
using SampleApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
 
namespace SampleApi.Services;

public class ProductService : IProductService
{
   private readonly IProductRepository _repo;
   public ProductService(IProductRepository repo) => _repo = repo;

   public Task<List<Product>> GetAllAsync() => _repo.GetAllAsync();
   public Task<Product?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
   public Task AddAsync(Product product) => _repo.AddAsync(product);
   public Task UpdateAsync(Product product) => _repo.UpdateAsync(product);
   public Task DeleteAsync(Product product) => _repo.DeleteAsync(product);
}

