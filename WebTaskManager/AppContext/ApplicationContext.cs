using Microsoft.EntityFrameworkCore;
using WebTaskManager.Model;

namespace WebTaskManager.AppContext
{
    //ApplicationContext — это центральный класс для работы с базой данных через Entity Framework Core.
    public class ApplicationContext : DbContext
    {
        //Абстракция БД: Позволяет работать с БД как с объектами C#
        //DbContext (контекст базы данных) Entity Framework Core, который:
        //Определяет подключение к базе данных
        //Управляет сущностями MyTaskModel
        private readonly string _dbFolderPath = @"C:\Users\GOLDNOVA\source\repos\WebTaskManager\WebTaskManager\DataBasetmpTask\";
        public string DefaultSchema => "MyTaskapi";
        //Схема по умолчанию: HasDefaultSchema("MyTaskapi") задаёт префикс для всех таблиц
        public ApplicationContext()
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database creation error: {ex.Message}");
                throw;
            }
        }

        public DbSet<MyTaskModel> MyTasks => Set<MyTaskModel>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Формируем полный путь к файлу БД
            string dbPath = Path.Combine(_dbFolderPath, "TestTaskss.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);// Установка схемы
            base.OnModelCreating(modelBuilder);// Важно вызывать базовый метод

            // Автоматическое применение всех конфигураций
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

        }

    }
}
