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
            builder1.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlite(builder1.Configuration.GetConnectionString("SqlConnection"));
            });

            builder1.Services.AddScoped<IMyTaskService, MyTaskServices>();

            //Add Global Exception handling

            builder1.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder1.Services.AddProblemDetails();

        }
    }
}
