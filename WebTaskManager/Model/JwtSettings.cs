namespace WebTaskManager.Model
{
    //бычный C# класс, который хранит настройки, нужные для работы с JWT (JSON Web Token).
    public class JwtSettings
    {
        public required string Secret { get; set; } //секретный ключ, который используется для подписи JWT токена
        public required string Issuer { get; set; } //Обычно это адрес API или сервиса.
        public required string Audience { get; set; } //для какой аудитории предназначен токен.
        public int TokenExpiryInMinutes { get; set; } //Время жизни токена в минутах.
    }
}
