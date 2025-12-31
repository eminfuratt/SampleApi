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
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _service;

        public ContactsController(IContactService service) => _service = service;

        //Veritabanındaki kişileri listeler 
        [HttpGet]
        public async Task<ActionResult<List<Contact>>> GetAll()
        {
            var contacts = await _service.GetAllAsync();
            return Ok(contacts);
        }

        //Belirtilen ID'ye sahip kişi bilgileri gelir
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetById(int id)
        {
            var contact = await _service.GetByIdAsync(id);
            if (contact == null)
                return NotFound();
            return Ok(contact);
        }

        //Yeni kişinin kaydı oluşur
        [HttpPost]
        public async Task<ActionResult> Create(CreateContactDto dto)
        {
            var contact = new Contact
            {
                Type = dto.Type,
                Value = dto.Value,
                UserId = dto.UserId
            };
            await _service.AddAsync(contact);
           
            return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact);


        }

        //Belirtilen ID değerine ait kişi bilgilerini günceller
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateContactDto dto)
        {
           var result = await _service.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        //Belirtilen ID’ye sahip kişiyi siler.
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
