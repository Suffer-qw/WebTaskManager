using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebTaskManager.AppContext;
using WebTaskManager.Contracts;
using WebTaskManager.Interface;
using WebTaskManager.Model;

namespace WebTaskManager.Services
{
    // Использование шаблона проектирования DTO
    // (DTO (Data Transfer Object) — это шаблон проектирования,
    // используемый для передачи данных между слоями приложения (например, между клиентом и сервером).)
    /*
     * // Input DTO (запрос на создание)
     * public record CreateProductRequest(
     * [Required] string Name,
     * [Range(0, 1000)] decimal Price
     * );
     * // Output DTO (ответ)
     * public record ProductResponse(
     * int Id,
     * string Name,
     * decimal Price
     * );
    */
    public class MyTaskServices : IMyTaskService
    {
        // readonly - запрещает редактировать переменную после создания и даёт понять другим программистом что этого делать не надо 
        private readonly ApplicationContext _context;

        private readonly ILogger<MyTaskServices> _logger;
        // CRUD операции для задач (Create, Read, Update, Delete)
        // Работа с БД через Entity Framework Core
        // Асинхронные операции
        public MyTaskServices(ApplicationContext context, ILogger<MyTaskServices> logger)
        {
            _context = context; // доступ к базе данных что бы читать и созранять
            _logger = logger; // журнал ошибок 
        }
        // Внедрение зависимостей (DB Context и Logger)

        public async Task<MyTaskResponse?> AddMyTaskAsync(CreateMyTaskRequest request, Guid userId)
        {
            try
            {
                // Найди статус по имени
                var status = await _context.TaskStatus
                    .FirstOrDefaultAsync(s => s.Status == request.Status);
                if (status == null)
                {
                    _logger.LogWarning($"Статус '{request.Status}' не найден");
                    return null;
                }

                var mytask = new MyTaskModel
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Type = request.Type,
                    TaskStatusId = status.Id, // записываем Id связанного статуса
                    TaskStatus = status,
                    UserId = userId,  // Присваиваем UserId из токена
                    // UserId Всегда равен {e09bd247-164c-4278-9e9f-f5fd1247a2c7}, вызыывает ошибку 
                    User = await _context.UserProfile.FirstAsync(s => s.Id == userId)
                };

                _context.MyTasks.Add(mytask);
                await _context.SaveChangesAsync();

                return new MyTaskResponse
                {
                    Id = mytask.Id,
                    Name = mytask.Name,
                    Description = mytask.Description,
                    Type = mytask.Type,
                    Status = mytask.TaskStatus.Status // возвращаем имя статуса пользователю
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        // Получение списка задач только для текущего пользователя
        public async Task<List<MyTaskResponse>?> GetAllMyTaskAsync(Guid userId)
        {
            try
            {
                var mytasks = await _context.MyTasks
                    .Include(x => x.TaskStatus)
                    .Where(x => x.UserId == userId)  // Фильтрация по UserId
                    .ToListAsync();

                return mytasks.Select(mytask => new MyTaskResponse
                {
                    Id = mytask.Id,
                    Name = mytask.Name,
                    Description = mytask.Description,
                    Type = mytask.Type,
                    Status = mytask.TaskStatus.Status,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        // Получение задачи по ID, только если она принадлежит пользователю
        public async Task<MyTaskResponse?> GetMyTaskByIdAsync(Guid myTaskId, Guid userId)
        {
            try
            {
                var mytask = await _context.MyTasks
                    .Include(t => t.TaskStatus)
                    .FirstOrDefaultAsync(x => x.Id == myTaskId && x.UserId == userId);  // Фильтрация по UserId
                if (mytask == null)
                {
                    _logger.LogInformation($"Задача с ID {myTaskId} не найдена или не принадлежит пользователю {userId}");
                    return null;
                }
                return new MyTaskResponse
                {
                    Id = mytask.Id,
                    Name = mytask.Name,
                    Description = mytask.Description,
                    Type = mytask.Type,
                    Status = mytask.TaskStatus.Status,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        // Обновление задачи, только если она принадлежит пользователю
        public async Task<MyTaskResponse?> UpdateMyTaskAsync(Guid myTaskId, UpdateMyTaskRequest updateRequest, Guid userId)
        {
            try
            {
                var existingTask = await _context.MyTasks
                    .Include(t => t.TaskStatus)
                    .FirstOrDefaultAsync(x => x.Id == myTaskId && x.UserId == userId);  // Фильтрация по UserId
                if (existingTask == null)
                {
                    _logger.LogInformation($"Задача с ID {myTaskId} не найдена или не принадлежит пользователю {userId}");
                    return null;
                }

                // Найди новый статус по имени
                var newStatus = await _context.TaskStatus
                    .FirstOrDefaultAsync(s => s.Status == updateRequest.Status);
                if (newStatus == null)
                {
                    _logger.LogWarning($"Статус '{updateRequest.Status}' не найден");
                    return null;
                }

                existingTask.Name = updateRequest.Name;
                existingTask.Description = updateRequest.Description;
                existingTask.Type = updateRequest.Type;
                existingTask.TaskStatusId = newStatus.Id;  // Обновляем ID статуса
                existingTask.TaskStatus = newStatus;  // Обновляем навигационное свойство

                await _context.SaveChangesAsync();
                return new MyTaskResponse
                {
                    Id = existingTask.Id,
                    Name = existingTask.Name,
                    Description = existingTask.Description,
                    Type = existingTask.Type,
                    Status = existingTask.TaskStatus.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        // Удаление задачи, только если она принадлежит пользователю
        public async Task<bool> DeleteMyTaskAsync(Guid myTaskId, Guid userId)
        {
            try
            {
                var mytask = await _context.MyTasks
                    .FirstOrDefaultAsync(x => x.Id == myTaskId && x.UserId == userId);  // Фильтрация по UserId
                if (mytask == null)
                {
                    _logger.LogInformation($"Задача с ID {myTaskId} не найдена или не принадлежит пользователю {userId}");
                    return false;
                }

                _context.MyTasks.Remove(mytask);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Задача с ID {myTaskId} удалена");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}