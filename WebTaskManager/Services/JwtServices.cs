using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebTaskManager.Model;

namespace WebTaskManager.Services
{
    public class JwtServices
    {
        //JwtServices отвечает за создание JWT токенов для авторизации пользователей.
        //1. Получает настройки JWT (Secret, Issuer, Audience, время жизни) из appsettings.json.
        //2. Формирует токен с нужными данными о пользователе(claims).
        //3. Подписывает токен секретным ключом.
        //4. Возвращает готовую строку токена, которую можно отдать клиенту.
        private readonly JwtSettings _settings;

        public JwtServices(IConfiguration configuration)
        {
            _settings = configuration.GetSection("Jwt").Get<JwtSettings>()!;
            //Через IConfiguration получаем секцию "Jwt" из appsettings.json и преобразуем её в объект JwtSettings.
        }

        public string GenerateToken(UserProfileModel model)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Id.ToString()),  // Добавляем Id как Sub (уникальный идентификатор)
                new Claim(ClaimTypes.NameIdentifier, model.Name),  // Оставляем Name
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            //Берём Secret из настроек, преобразуем его в байты UTF-8, оборачиваем в SymmetricSecurityKey.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //Указываем, как будем подписывать токен

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
