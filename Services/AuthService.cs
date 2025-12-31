using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleApi.Models;
using SampleApi.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        //Kullanıcı kayıt işlemi 
        public async Task<string> RegisterAsync(string name, string email, string password)
        {

            var userExists = await _repo.GetByEmailAsync(email);
            if (userExists != null)
                throw new Exception("Bu e-posta zaten kayıtlı.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                Role = "User" // Varsayılan rol
            };

            await _repo.AddAsync(user);

            return GenerateToken(user);
        }

        //Kullanıcı giriş işlemi
        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _repo.GetByEmailAsync(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Email veya şifre hatalı.");

            return GenerateToken(user);
        }

        //JWT Token üretme metodu
        private string GenerateToken(User user)
        {
            // Null role varsa default olarak User ata
            var role = string.IsNullOrEmpty(user.Role) ? "User" : user.Role;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            //JWT imza anahtarı 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresMinutes = int.Parse(_config["Jwt:ExpiresMinutes"] ?? "60");

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
