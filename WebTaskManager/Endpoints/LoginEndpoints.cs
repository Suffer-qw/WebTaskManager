using System;
using Microsoft.EntityFrameworkCore;
using WebTaskManager.AppContext;
using WebTaskManager.Contracts;
using WebTaskManager.Interface;
using WebTaskManager.Model;
using WebTaskManager.Services;

namespace WebTaskManager.Endpoints
{
    public static class LoginEndpoints
    {
        public static IEndpointRouteBuilder MapLoginEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async (CreateUserProfileRequest login, JwtServices jwtService, ILoginService loginService) =>
            {
                var user = await loginService.AuthenticateAsync(login);
                if (user == null)
                    return Results.Unauthorized();

                var token = jwtService.GenerateToken(user);
                return Results.Ok(new { token });
            });

            app.MapPost("/register", async (CreateUserProfileRequest registerModel, ILoginService loginService) =>
            {
                var success = await loginService.RegisterAsync(registerModel);
                if (!success)
                    return Results.BadRequest("User already exists");

                return Results.Ok("User registered successfully");
            });

            return app;
        }
    }
}