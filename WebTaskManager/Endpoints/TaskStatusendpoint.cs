using WebTaskManager.Interface;

namespace WebTaskManager.Endpoints
{
    public static class TaskStatusendpoint
    {
        public static IEndpointRouteBuilder MapTaskStatusEndPoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/UserProfileGetAll", async (ITaskStatusService TaskStatusService) =>
            {
                var result = await TaskStatusService.GetAllStatusAsync();
                return Results.Ok(result);
            });

            return app;
        }
    }
}
