﻿using Microsoft.EntityFrameworkCore;
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
        private readonly string _dbFolderPath;
        

        public string DefaultSchema => "MyTaskapi";
        public ApplicationContext()
        {
            var projectRoot = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, "..", "..", ".."));
            _dbFolderPath = Path.Combine(projectRoot, "DataBasetmpTask\\");
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
        public DbSet<UserProfileModel> UserProfile => Set<UserProfileModel>();
        public DbSet<TaskStatusModel> TaskStatus => Set<TaskStatusModel>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Формируем полный путь к файлу БД
            string dbPath = Path.Combine(_dbFolderPath, "TestTaskss.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);// Важно вызывать базовый метод, что бы Ef не потеряла системную конфигурацию
            //заранее добовляем несколько статусов 
            modelBuilder.Entity<TaskStatusModel>().HasData(
                new TaskStatusModel { Id = Guid.NewGuid(), Status = "Новая" },
                new TaskStatusModel { Id = Guid.NewGuid(), Status = "В работе" },
                new TaskStatusModel { Id = Guid.NewGuid(), Status = "Завершена" }
                );

            // Автоматическое применение всех конфигураций
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

        }

    }
}
