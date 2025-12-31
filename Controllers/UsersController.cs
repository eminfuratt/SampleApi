using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;
using SampleApi.Models.DTOs;
using SampleApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service) => _service = service;

        

        // Tüm kullanıcıları getirir
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        // Girilen ID ye ait kullanıcıyı getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // Yeni kullanıcı ekleme
        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            await _service.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // Kullanıcı güncelleme (DTO ile)
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateUserDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result)
                return NotFound();  

            return NoContent();
        }

        // Kullanıcı silme
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
