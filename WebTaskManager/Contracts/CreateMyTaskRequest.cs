namespace WebTaskManager.Contracts
{
    public class CreateMyTaskRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
    }
}
