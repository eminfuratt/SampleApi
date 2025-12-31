using Microsoft.AspNetCore.Http;
using SampleApi.Models;
using SampleApi.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(string? role = null, int? userId = null);
        Task<Order?> GetOrderByIdAsync(int id, string? role = null, int? userId = null);
        Task<Order> CreateOrderAsync(int currentUserId, int productId, int quantity, string? role = null, int? targetUserId = null);

        Task<bool> UpdateOrderAsync(int id, Order order);
        Task<Order?> UpdateOwnOrderAsync(int userId, int orderId, UpdateOrderDto dto);

        Task<bool> DeleteOrderAsync(int id);

        Task<int> UploadOrdersFromJsonAsync(IFormFile file);
        Task<int> UploadOrdersFromCsvAsync(IFormFile file);

        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);

        // Product bilgileri dönen method
        Task<IEnumerable<Order>> GetOrdersWithProductsByUserIdAsync(int userId);
    }
}
