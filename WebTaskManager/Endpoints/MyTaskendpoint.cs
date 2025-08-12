using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using WebTaskManager.Contracts;
using WebTaskManager.Interface;

namespace WebTaskManager.Endpoints
{
    public static class MyTaskendpoint
    {
        private static Guid GetuserIdClaim(HttpContext httpContext)
        {
            foreach(var claim in httpContext.User.Claims)
            {
                Console.WriteLine($"tyt clam {claim.Value} -- {claim.Type} -- {claim.Subject}");
            }    
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out Guid userId))
                return Guid.Parse("e09bd247-164c-4278-9e9f-f5fd1247a2c7");
            return Guid.Parse(userIdClaim);
        }
        public static IEndpointRouteBuilder MapMyTaskEndPoint(this IEndpointRouteBuilder app)
        {
            //create mytask 
            app.MapPost("/myTasksCreate", async (CreateMyTaskRequest CreateMyTask, IMyTaskService myTaskService, HttpContext httpContext) =>
            {
                var task = await myTaskService.AddMyTaskAsync(CreateMyTask, GetuserIdClaim(httpContext));
                return Results.Created($"/api/v1/mytask/{task.Id}", task);
            })
                .RequireAuthorization();

            //Get All books

            app.MapGet("/myTasksGetAll", async (IMyTaskService myTaskService, HttpContext httpContext) =>
            {
                var result = await myTaskService.GetAllMyTaskAsync(GetuserIdClaim(httpContext));
                return Results.Ok(result);
            })
                .RequireAuthorization();

            //Get by id

            app.MapGet("/mytask/{taskid}", async (Guid taskid, IMyTaskService myTaskService, HttpContext httpContext) =>
            {
                var result = await myTaskService.GetMyTaskByIdAsync(taskid, GetuserIdClaim(httpContext));
                if (result == null)
                    return Results.NotFound();
                return Results.Ok(result);  
            })
                .RequireAuthorization();

            //Update Task

            app.MapPut("/mytask/{taskid}", async (Guid taskid,UpdateMyTaskRequest update, IMyTaskService myTaskService, HttpContext httpContext) =>
            {
                var result = await myTaskService.UpdateMyTaskAsync(taskid, update, GetuserIdClaim(httpContext));
                if (result == null)
                    return Results.NotFound();
                return Results.Ok(result);
            })
                .RequireAuthorization();

            //Delete Task

            app.MapDelete("/mytask/{taskid}", async (Guid taskid,IMyTaskService myTaskService, HttpContext httpContext) =>
            {
                var result = await myTaskService.DeleteMyTaskAsync(taskid, GetuserIdClaim(httpContext));
                if (!result)
                    return Results.NotFound();
                return Results.NoContent();
            })
                .RequireAuthorization();

            return app;
        }

    }
}
