using WebTaskManager.Interface;
using WebTaskManager.Services;

namespace WebTaskManager.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddApplicationServies(this IHostApplicationBuilder builder1)
        {
            builder1.Services.AddScoped<IMyTaskService, MyTaskServices>();
        }
    }
}
