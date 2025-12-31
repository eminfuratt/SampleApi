using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;
using SampleApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service) => _service = service;

        //Ürünleri listeler
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        //Verilen ID ye göre ürünü getirir
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        //Ürün ekleme 
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            await _service.AddAsync(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        //Ürün güncelleme
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            await _service.UpdateAsync(product);
            return NoContent();
        }

        //Ürün silme
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            await _service.DeleteAsync(product);
            return NoContent();
        }
    }
}
