using Microsoft.AspNetCore.Http;
using SampleApi.Models;
using SampleApi.Models.DTOs;
using SampleApi.Repositories;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System;


namespace SampleApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly RabbitMqPublisherService _publisher;

        public OrderService(IOrderRepository repo, RabbitMqPublisherService publisher)
        {
            _repo = repo;
            _publisher = publisher;
        } 

        // Tüm orderları getir (Admin tümünü, user sadece kendi orderlarını)
        public async Task<IEnumerable<Order>> GetAllOrdersAsync(string? role = null, int? userId = null)
        {
            var orders = await _repo.GetAllAsync();

            if (role != "Admin" && userId.HasValue)
                orders = orders.Where(o => o.UserId == userId.Value).ToList();

            return orders;
        }

        // Admin tüm orderları görür user sadece kendi orderını görür
        public async Task<Order?> GetOrderByIdAsync(int id, string? role = null, int? userId = null)
        {
            var order = await _repo.GetByIdAsync(id);
            if (order == null) return null;

            if (role != "Admin" && userId.HasValue && order.UserId != userId.Value)
                return null;

            return order;
        }

        // Admin başkası için de order oluşturabilir, user sadece kendisi için
        public async Task<Order> CreateOrderAsync(
            int currentUserId,
            int productId,
            int quantity,
            string? role = null,
            int? targetUserId = null)
        {
            if (role != "Admin" && targetUserId.HasValue)
            throw new UnauthorizedAccessException(
             "User başka bir kullanıcı adına sipariş oluşturamaz."
            );

        int finalUserId = (role == "Admin" && targetUserId.HasValue)
        ? targetUserId.Value
        : currentUserId;

        if (quantity <= 0)
        quantity = 1;

        var order = new Order
        {
            UserId = finalUserId,
            ProductId = productId,
            Quantity = quantity,
            OrderDate = DateTime.UtcNow
        };

        await _repo.AddAsync(order);

        //Event hazırla
        var email = "kullanici@email.com";
        var totalPrice = quantity * 100;  

        //RabbitMQ event gönder
        await _publisher.PublishOrderPurchasedAsync(
            new OrderPurchaseEventDto
            {
                OrderId = order.Id,
                Email = email,
                TotalPrice = totalPrice
            });

        return order;
}
        // Admin order günceller (her şeyi değiştirebilir)
        public async Task<bool> UpdateOrderAsync(int id, Order order)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.UserId = order.UserId;
            existing.ProductId = order.ProductId;
            existing.Quantity = order.Quantity;
            existing.OrderDate = order.OrderDate;

            await _repo.UpdateAsync(existing);
            return true;
        }

        // Kullanıcı kendi orderını günceller – DTO ile ürün ve miktarını değiştrir
        public async Task<Order?> UpdateOwnOrderAsync(int userId, int orderId, UpdateOrderDto dto)
        {
            var existing = await _repo.GetByIdAsync(orderId);
            if (existing == null || existing.UserId != userId)
                return null;

            existing.ProductId = dto.ProductId;
            existing.Quantity = dto.Quantity;
            existing.OrderDate = DateTime.UtcNow;

            await _repo.UpdateAsync(existing);
            return existing;
        }

        // Order siler
        public async Task<bool> DeleteOrderAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            await _repo.DeleteAsync(existing);
            return true;
        }

        // JSON yükleme
        public async Task<int> UploadOrdersFromJsonAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var orders = await JsonSerializer.DeserializeAsync<List<Order>>(stream)
                         ?? new List<Order>();

            foreach (var order in orders)
                await _repo.AddAsync(order);

            return orders.Count;
        }

        // CSV yükleme
        public async Task<int> UploadOrdersFromCsvAsync(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            bool isHeader = true;
            int count = 0;
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                var parts = line.Split(',');

                if (parts.Length < 3)
                    continue;

                var order = new Order
                {
                    UserId = int.Parse(parts[0]),
                    ProductId = int.Parse(parts[1]),
                    OrderDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Quantity = parts.Length >= 4 ? int.Parse(parts[3]) : 1
                };

                await _repo.AddAsync(order);
                count++;
            }

            return count;
        }

        // Kullanıcının kendi tüm orderları
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _repo.GetAllAsync();
            return orders.Where(o => o.UserId == userId);
        }

        // Kullanıcının orderlarını ürün bilgileri ile getirir
        public async Task<IEnumerable<Order>> GetOrdersWithProductsByUserIdAsync(int userId)
        {
            return await _repo.GetOrdersWithProductsByUserIdAsync(userId);
        }
    }
}
