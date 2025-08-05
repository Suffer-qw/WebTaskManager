using Microsoft.EntityFrameworkCore;
using WebTaskManager.AppContext;
using WebTaskManager.Exceptions;
using WebTaskManager.Interface;
using WebTaskManager.Services;

namespace WebTaskManager.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddApplicationServies(this IHostApplicationBuilder builder1)
        {
            builder1.Services.AddDbContext<ApplicationContext>();

            builder1.Services.AddScoped<IMyTaskService, MyTaskServices>();
            builder1.Services.AddScoped<IUserProfileService, UserProfileServices>();

            //Add Global Exception handling

            builder1.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder1.Services.AddProblemDetails();

        }
    }
}
