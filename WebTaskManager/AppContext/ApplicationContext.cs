using Microsoft.EntityFrameworkCore;
using WebTaskManager.Model;

namespace WebTaskManager.AppContext
{
    //ApplicationContext — это центральный класс для работы с базой данных через Entity Framework Core.
    public class ApplicationContext(DbContextOptions <ApplicationContext> options) : DbContext(options)
    {
        //Абстракция БД: Позволяет работать с БД как с объектами C#
        //DbContext (контекст базы данных) Entity Framework Core, который:
        //Определяет подключение к базе данных
        //Управляет сущностями MyTaskModel
        public string DefaultSchema => "MyTaskapi";
        //Схема по умолчанию: HasDefaultSchema("MyTaskapi") задаёт префикс для всех таблиц

        public DbSet<MyTaskModel> MyTasks { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);// Установка схемы
            base.OnModelCreating(modelBuilder);// Важно вызывать базовый метод

            // Автоматическое применение всех конфигураций
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

        }

    }
}
