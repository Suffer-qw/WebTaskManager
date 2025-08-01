namespace WebTaskManager.Contracts
{
    public class MyTaskResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
    }
}
