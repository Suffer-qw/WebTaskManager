using WebTaskManager.Contracts;

namespace WebTaskManager.Interface
{
    public interface ITaskStatusService
    {
        Task<List<TaskStatusResponse>?> GetAllStatusAsync();
        Task<TaskStatusResponse?> AddStatusAsync(CreateTaskStatusRequest request);
        Task<TaskStatusResponse?> GetStatusByIdAsync(Guid StatusId);
        Task<TaskStatusResponse?> UpdateStatusAsync(Guid StatusId, UpdateTaskStatusRequest updateRequest);
        Task<bool> DeleteStatusAsync(Guid StatusId);



    }
}
