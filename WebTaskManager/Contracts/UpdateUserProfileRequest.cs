namespace WebTaskManager.Contracts
{
    public class UpdateUserProfileRequest
    {
        public required string Name { get; set; }

        public required string Key { get; set; }
    }
}
