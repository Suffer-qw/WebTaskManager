namespace WebTaskManager.Contracts
{
    public class CreateUserProfileRequest
    {
        public required string Name { get; set; }

        public required string Key { get; set; }
    }
}
