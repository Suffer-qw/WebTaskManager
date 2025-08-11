using WebTaskManager.Contracts;
using WebTaskManager.Model;

namespace WebTaskManager.Interface
{
    public interface ILoginService
    {
        Task<UserProfileModel?> AuthenticateAsync(CreateUserProfileRequest login);
        Task<bool> RegisterAsync(CreateUserProfileRequest registerModel);
    }
}
