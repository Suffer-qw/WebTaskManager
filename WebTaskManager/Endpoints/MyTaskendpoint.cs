using WebTaskManager.Contracts;
using WebTaskManager.Interface;

namespace WebTaskManager.Endpoints
{
    public static class MyTaskendpoint
    {
        //create mytask 
        public static IEndpointRouteBuilder MapMyTaskEndPoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/myTasksCreate", async (CreateMyTaskRequest CreateMyTask, IMyTaskService myTaskService) =>
            {
                var result = await myTaskService.AddMyTaskAsync(CreateMyTask);
                return Results.Created($"/mytask/{result.Id}", result);
            });

            //Get All books

            app.MapGet("/myTasksGetAll", async (IMyTaskService myTaskService) =>
            {
                var result = await myTaskService.GetAllMyTaskAsync();
                return Results.Ok(result);
            });

            //Get by id

            app.MapGet("/mytask/{taskid}", async (Guid taskid, IMyTaskService myTaskService) =>
            {
                var result = await myTaskService.GetMyTaskByIdAsync(taskid);
                if (result == null)
                    return Results.NotFound();
                return Results.Ok(result);  
            });

            //Update Task

            app.MapPut("/mytask/{taskid}", async (Guid taskid,UpdateMyTaskRequest update, IMyTaskService myTaskService) =>
            {
                var result = await myTaskService.UpdateMyTaskAsync(taskid, update);
                if (result == null)
                    return Results.NotFound();
                return Results.Ok(result);
            });

            //Delete Task

            app.MapDelete("/mytask/{taskid}", async (Guid taskid,IMyTaskService myTaskService) =>
            {
                var result = await myTaskService.DeleteMyTaskAsync(taskid);
                if (!result)
                    return Results.NotFound();
                return Results.NoContent();
            });

            return app;
        }

    }
}
