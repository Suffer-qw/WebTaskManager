using WebTaskManager.Contracts;
using WebTaskManager.Interface;

namespace WebTaskManager.Endpoints
{
    public static class UserProfileendpoint
    {
        public static IEndpointRouteBuilder MapUserProfileEndPoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/UserProfileCreate", async (CreateUserProfileRequest CreateUser, IUserProfileService UserProfileService) =>
            {
                var result = await UserProfileService.AddUserProfileAsync(CreateUser);
                return Results.Created($"/User/{result.Id}", result);
            });
            app.MapGet("/UserProfileGetAll", async ( IUserProfileService UserProfileService) =>
            {
                var result = await UserProfileService.GetAllUserProfileAsync();
                return Results.Ok(result);
            });
            app.MapGet("/UserProfile/{UserId}", async (Guid Userid, IUserProfileService UserProfileService) =>
            {
                var result = await UserProfileService.GetUserProfileByIdAsync(Userid);
                if(result == null)
                    return Results.NotFound();
                return Results.Ok(result);
            });
            app.MapPut("/UserProfile/{UserId}", async (UpdateUserProfileRequest update,Guid Userid, IUserProfileService UserProfileService) =>
            {
                var result = await UserProfileService.UpdateUserProfileAsync(Userid,update);
                if (result == null)
                    return Results.NotFound();
                return Results.Ok(result);
            });
            app.MapDelete("/UserProfile/{UserId}", async (Guid Userid, IUserProfileService UserProfileService) =>
            {
                var result = await UserProfileService.DeleteUserProfileAsync(Userid);
                if (!result)
                    return Results.NotFound();
                return Results.NoContent();
            });
            return app;
        }
    }
}
