using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebTaskManager.AppContext;
using WebTaskManager.Contracts;
using WebTaskManager.Interface;

namespace WebTaskManager.Services
{
    public class UserProfileServices : IUserProfileService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UserProfileServices> _logger;

        public UserProfileServices(ApplicationContext context, ILogger<UserProfileServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserProfileResponse> AddUserProfileAsync(CreateUserProfileRequest request)
        {
            try
            {
                var user = new Model.UserProfileModel
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Key = request.Key
                };
                _context.UserProfile.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"user with id{user.Id} added database");
                return new UserProfileResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Key = user.Key
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<List<UserProfileResponse>> GetAllUserProfileAsync()
        {
            try
            {
                var user = await _context.UserProfile.ToListAsync();
                return user.Select(x => new UserProfileResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Key = x.Key
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        

        public async Task<UserProfileResponse> GetUserProfileByIdAsync(Guid UserProfileId)
        {
            try
            {
                var user = await _context.UserProfile.FindAsync(UserProfileId);
                if (user == null)
                {
                    // _logger.LogInformation($"");
                    return null;
                }
                return new UserProfileResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Key = user.Key
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<UserProfileResponse> UpdateUserProfileAsync(Guid UserProfileId, UpdateUserProfileRequest updateRequest)
        {
            try
            {
                var existingUser = await _context.UserProfile.FindAsync(UserProfileId);
                if (existingUser == null)
                {
                    // _logger.LogInformation($"");
                    return null;
                }
                existingUser.Name = updateRequest.Name;
                existingUser.Key = updateRequest.Key;
                await _context.SaveChangesAsync();
                return new UserProfileResponse
                {
                    Id = existingUser.Id,
                    Name = existingUser.Name,
                    Key = existingUser.Key
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public  Task<bool> DeleteUserProfileAsync(Guid UserProfileId)
        {
            var user = _context.UserProfile.Find(UserProfileId);
            if(user == null)
            {
                // _logger.LogInformation($"");
                return Task.FromResult(false);
            }
            try
            {
                _context.UserProfile.Remove(user);
                _context.SaveChanges();
                // _logger.LogInformation($"");
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromResult(false);
            }
        }
    }
}
