using WebTaskManager.Contracts;

namespace WebTaskManager.Interface
{
    public interface IUserProfileService
    {
        Task<UserProfileResponse> AddUserProfileAsync(CreateUserProfileRequest request);
        Task<UserProfileResponse> GetUserProfileByIdAsync(Guid UserProfileId);
        Task<List<UserProfileResponse>> GetAllUserProfileAsync();
        Task<UserProfileResponse> UpdateUserProfileAsync(Guid UserProfileId, UpdateUserProfileRequest updateRequest);

        Task<bool> DeleteUserProfileAsync(Guid UserProfileId);
    }
}
