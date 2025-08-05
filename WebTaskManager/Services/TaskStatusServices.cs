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

        public async Task<List<TaskStatusResponse>> GetAllStatusAsync()
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
    }
}
