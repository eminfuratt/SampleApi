using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;
using SampleApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _service;

        public AddressesController(IAddressService service) => _service = service;

        //Tüm adres kayıtlarını döndürür.
        [HttpGet]
        public async Task<ActionResult<List<Address>>> GetAll()
        {
            var addresses = await _service.GetAllAsync();
            return Ok(addresses);
        }

        //ID'ye göre adres arar
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetById(int id)
        {
            var address = await _service.GetByIdAsync(id);
            if (address == null)
                return NotFound();
            return Ok(address);
        }

        //Yeni bir adres oluşturur.
        [HttpPost]
        public async Task<ActionResult> Create(Address address)
        {
            await _service.AddAsync(address);
            return CreatedAtAction(nameof(GetById), new { id = address.Id }, address);
        }

        //Güncelleme işlemi yapar.
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Address address)
        {
            if (id != address.Id)
                return BadRequest();

            await _service.UpdateAsync(address);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
