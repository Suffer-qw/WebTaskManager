using Microsoft.EntityFrameworkCore;
using WebTaskManager.AppContext;
using WebTaskManager.Contracts;
using WebTaskManager.Interface;

namespace WebTaskManager.Services
{
    public class TaskStatusServices : ITaskStatusService
    {
        private readonly ApplicationContext _context;

        private readonly ILogger<TaskStatusServices> _logger;

        public TaskStatusServices(ApplicationContext context, ILogger<TaskStatusServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TaskStatusResponse?> AddStatusAsync(CreateTaskStatusRequest request)
        {
            try
            {
                var status = new Model.TaskStatusModel
                {
                    Id = Guid.NewGuid(),
                    Status = request.Status
                };
                _context.TaskStatus.Add(status);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"status with id{status.Id} added database");
                return new TaskStatusResponse
                {
                    Id = status.Id,
                    Status = status.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public async Task<List<TaskStatusResponse>?> GetAllStatusAsync()
        {
            try
            {
                var status = await _context.TaskStatus.ToListAsync();

                return status.Select(mytask => new TaskStatusResponse
                {
                    Id = mytask.Id,
                    Status = mytask.Status,

                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<TaskStatusResponse?> GetStatusByIdAsync(Guid StatusId)
        {
            try
            {
                var status = await _context.TaskStatus.FindAsync(StatusId);
                if (status == null)
                {
                    _logger.LogInformation($"status dont find by id");
                    return null;
                }
                return new TaskStatusResponse
                {
                    Id = status.Id,
                    Status = status.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<TaskStatusResponse?> UpdateStatusAsync(Guid StatusId, UpdateTaskStatusRequest updateRequest)
        {
            try
            {
                var existingstatus = await _context.TaskStatus.FindAsync(StatusId);
                if (existingstatus == null)
                {
                    _logger.LogInformation($"status dont find");
                    return null;
                }
                existingstatus.Status = updateRequest.Status;
                await _context.SaveChangesAsync();
                return new TaskStatusResponse
                {
                    Id = existingstatus.Id,
                    Status = existingstatus.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public Task<bool> DeleteStatusAsync(Guid StatusId)
        {
            var status = _context.TaskStatus.Find(StatusId);
            if (status == null)
            {
                _logger.LogInformation($"status dont find");
                return Task.FromResult(false);
            }
            try
            {
                _context.TaskStatus.Remove(status);
                _context.SaveChanges();
                _logger.LogInformation($"status Remove");
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
