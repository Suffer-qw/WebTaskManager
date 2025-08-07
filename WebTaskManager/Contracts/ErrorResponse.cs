namespace WebTaskManager.Contracts
{
    public class ErrorResponse
    {

        public required string Title { get; set; }

        public int StatusCode { get; set; } = 0;

        public required string Message { get; set; }

    }
}
