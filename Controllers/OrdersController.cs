using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;
using SampleApi.Services;
using SampleApi.Models.DTOs;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SampleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService service, ILogger<OrdersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // Admin tüm orderları görür
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            _logger.LogInformation("Admin - GetAll çağrıldı.");

            var orders = await _service.GetAllOrdersAsync("Admin");
            return Ok(orders);
        }

        // Kullanıcı kendi siparişlerini görür
        [Authorize]
        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetMyOrders()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            var orders = await _service.GetOrdersByUserIdAsync(currentUserId);
            return Ok(orders);
        }

        // Belirli orderı getirir (Admin her orderı görebilir, kullanıcı kendi orderını görebilir)
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var isAdmin = User.IsInRole("Admin");

            var order = await _service.GetOrderByIdAsync(id, isAdmin ? "Admin" : null, currentUserId);

            if (order == null)
            {
                _logger.LogWarning("Get: Order bulunamadı veya yetkisiz erişim. OrderId: {OrderId}, UserId: {UserId}", id, currentUserId);
                return NotFound("Order bulunamadı veya yetkisiz erişim.");
            }

            _logger.LogWarning("Get: Order getirildi. OrderId: {OrderId}", id);
            return Ok(order);
        }

        //Yeni order oluşturur. Admin başka kullanıcıya order ekleyebilir. User sadece kendine ekleyebilir
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var created = await _service.CreateOrderAsync(
                currentUserId: currentUserId,
                productId: dto.ProductId,
                quantity: dto.Quantity,
                role: null,
                targetUserId: null
            );

            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // Admin başka kullanıcı adına order oluşturur
        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> CreateByAdmin(CreateOrderByAdminDto dto)
        {
            var created = await _service.CreateOrderAsync(
                currentUserId: dto.UserId,
                productId: dto.ProductId,
                quantity: dto.Quantity,
                role: "Admin",
                targetUserId: dto.UserId
            );

            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // Kullanıcı kendi orderını günceller
        [Authorize]
        [HttpPut("my-order/{id}")]
        public async Task<IActionResult> UpdateOwnOrder(int id, UpdateOrderDto dto)
        {
           var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

           var updated = await _service.UpdateOwnOrderAsync(currentUserId, id, dto);

           if (updated == null)
             return NotFound("Order bulunamadı veya yetkisiz erişim.");

            return NoContent();
        }


        // Admin tüm orderları günceller
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order order)
        {
            if (id != order.Id)
            {
                _logger.LogWarning("Update: URL ID ve Body ID uyuşmuyor. Id: {Id}", id);
                return BadRequest("URL ID ve Body ID uyuşmuyor");
            }

            var updated = await _service.UpdateOrderAsync(id, order);

            if (!updated)
            {
                _logger.LogWarning("Update: Order bulunamadı. OrderId: {OrderId}", id);
                return NotFound();
            }
            _logger.LogWarning("Updaet: Order güncellendi. OrderId: {OrderId} ", id);
            return NoContent();
        }

        //Admin order siler 
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteOrderAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("Delete: Order bulunamadı. OrderId:{OrderId}", id);
                return NotFound();
            }
            _logger.LogWarning("Delete: Order silindi. OrderId: {OrderId}", id);
            return NoContent();
        }

        //JSON yükleme
        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadOrdersJson(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya bulunamadı.");

            var count = await _service.UploadOrdersFromJsonAsync(file);

            return Ok(new { message = $"{count} adet sipariş JSON'dan yüklendi." });
        }

        //CSV yükleme
        [Authorize(Roles = "Admin")]
        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadOrdersCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya bulunamadı.");

            var count = await _service.UploadOrdersFromCsvAsync(file);

            return Ok(new { message = $"{count} adet sipariş CSV'den yüklendi." });
        }
    }
}
