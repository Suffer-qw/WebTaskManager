namespace WebTaskManager.Contracts
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Key { get; set; }
    }
}
