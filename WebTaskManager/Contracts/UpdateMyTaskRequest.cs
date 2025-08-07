namespace WebTaskManager.Contracts
{
    public class UpdateMyTaskRequest
    {
        public required string Name { get; set; }

        public required string Description { get; set; }

        public required string Type { get; set; }

        public required string Status { get; set; }
    }
}
