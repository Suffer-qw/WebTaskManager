using WebTaskManager.Contracts;
using WebTaskManager.Interface;

namespace WebTaskManager.Endpoints
{
    public static class TaskStatusendpoint
    {
        public static IEndpointRouteBuilder MapTaskStatusEndPoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/CreateTaskStatus", async (CreateTaskStatusRequest createTaskStatus,ITaskStatusService TaskStatusService) =>
            {
                var result = await TaskStatusService.AddStatusAsync(createTaskStatus);
                return Results.Ok(result);
            });
            app.MapGet("/TaskStatusGetAll", async (ITaskStatusService TaskStatusService) =>
            {
                var result = await TaskStatusService.GetAllStatusAsync();
                return Results.Ok(result);
            });
            app.MapGet("/TaskStatus/{StatusId}", async (Guid Statusid, ITaskStatusService TaskStatusService) =>
            {
                var result = await TaskStatusService.GetStatusByIdAsync(Statusid);
                if (result == null)
                    return Results.NotFound();
                return Results.Ok(result);
            });
            app.MapPut("/TaskStatus/{StatusId}", async (UpdateTaskStatusRequest update,Guid Statusid, ITaskStatusService TaskStatusService) =>
            {
                var result = await TaskStatusService.UpdateStatusAsync(Statusid, update);
                if (result == null)
                    return Results.NotFound();
                return Results.Ok(result);
            });
            app.MapDelete("/TaskStatus/{StatusId}", async (Guid Statusid, ITaskStatusService TaskStatusService) =>
            {
                var result = await TaskStatusService.DeleteStatusAsync(Statusid);
                if (!result)
                    return Results.NotFound();
                return Results.NoContent();
            });

            return app;
        }
    }
}
