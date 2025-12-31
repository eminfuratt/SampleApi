using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Models.DTOs;
using SampleApi.Services;

namespace SampleApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        //Kullanıcı kayıt olmak için endpointe istek atar
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var token = await _auth.RegisterAsync(dto.Name, dto.Email, dto.Password);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Kullanıcı giriş yapar 
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var token = await _auth.LoginAsync(dto.Email, dto.Password);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
