using WebTaskManager.Contracts;
using WebTaskManager.Interface;

namespace WebTaskManager.Endpoints
{
    public static class MyTaskendpoint
    {
        //create mytask 
        public static IEndpointRouteBuilder MapMyTaskEndPoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/myTasks", async (CreateMyTaskRequest CreateMyTask, IMyTaskService myTaskService) =>
            {
                var result = await myTaskService.AddMyTaskAsync(CreateMyTask);
                return Results.Created($"/mytask/{result.Id}", result);
            });
            return app;
        }
    }
}
