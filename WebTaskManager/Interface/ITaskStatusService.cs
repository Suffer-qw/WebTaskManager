using WebTaskManager.Contracts;

namespace WebTaskManager.Interface
{
    public interface ITaskStatusService
    {
        Task<List<TaskStatusResponse>> GetAllStatusAsync();
    }
}
