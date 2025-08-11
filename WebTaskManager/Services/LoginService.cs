using Microsoft.EntityFrameworkCore;
using WebTaskManager.AppContext;
using WebTaskManager.Contracts;
using WebTaskManager.Interface;
using WebTaskManager.Model;

namespace WebTaskManager.Services
{
    public class LoginService : ILoginService
    {

        private readonly ApplicationContext _context;
        private readonly ILogger<LoginService> _logger;

        public LoginService(ApplicationContext context, ILogger<LoginService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserProfileModel?> AuthenticateAsync(CreateUserProfileRequest login)
        {
            try
            {
                return await _context.UserProfile
                    .FirstOrDefaultAsync(u => u.Name == login.Name && u.Key == login.Key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при аутентификации");
                return null;
            }
        }

        public async Task<bool> RegisterAsync(CreateUserProfileRequest registerModel)
        {
            try
            {
                if (await _context.UserProfile.AnyAsync(u => u.Name == registerModel.Name && u.Key == registerModel.Key))
                    return false;

                var user = new Model.UserProfileModel
                {
                    Id = Guid.NewGuid(),
                    Name = registerModel.Name,
                    Key = registerModel.Key
                };

                _context.UserProfile.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации");
                return false;
            }
        }
    }
}
