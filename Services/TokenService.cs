using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SampleApi.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        //Token oluşturma metodu
        public string CreateToken(string username)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JwtSettings:Secret eksik");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var expiresMinutes = Convert.ToDouble(jwtSettings["ExpiresMinutes"] ?? "120");

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
