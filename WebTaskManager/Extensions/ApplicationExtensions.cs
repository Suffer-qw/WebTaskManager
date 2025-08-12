using Microsoft.EntityFrameworkCore;
using WebTaskManager.AppContext;
using WebTaskManager.Interface;
using WebTaskManager.Services;

namespace WebTaskManager.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddApplicationServies(this IHostApplicationBuilder builder1)
        {
            builder1.Services.AddDbContext<ApplicationContext>();

            //Scoped-Сервисы , регистрация сервисов приложения которые отвечают за бизнес логику в виде (интерфейс, реализация) 
            builder1.Services.AddScoped<IMyTaskService, MyTaskServices>();
            builder1.Services.AddScoped<IUserProfileService, UserProfileServices>();
            builder1.Services.AddScoped<ITaskStatusService, TaskStatusServices>();
            builder1.Services.AddScoped<ILoginService,LoginService>();

            builder1.Services.AddScoped<JwtServices>();


            builder1.Services.AddProblemDetails();

        }
    }
}
