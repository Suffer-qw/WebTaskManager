namespace WebTaskManager.Contracts
{
    public class TaskStatusResponse
    {
        public Guid Id { get; set; }
        public required string Status { get; set; }
    }
}
