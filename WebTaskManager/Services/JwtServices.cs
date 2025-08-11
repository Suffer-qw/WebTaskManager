using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebTaskManager.Model;

namespace WebTaskManager.Services
{
    public class JwtServices
    {
        private readonly JwtSettings _settings;

        public JwtServices(IConfiguration configuration)
        {
            _settings = configuration.GetSection("Jwt").Get<JwtSettings>();
        }

        public string GenerateToken(UserProfileModel model)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Id.ToString()),  // Добавляем Id как Sub (уникальный идентификатор)
                new Claim(ClaimTypes.NameIdentifier, model.Name),  // Оставляем Name
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_settings.TokenExpiryInMinutes),
                SigningCredentials = credentials,
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);

        }
    }
}
