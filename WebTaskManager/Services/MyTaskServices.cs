﻿using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebTaskManager.AppContext;
using WebTaskManager.Contracts;
using WebTaskManager.Interface;
using WebTaskManager.Model;
namespace WebTaskManager.Services
{

    //Использование шаблона проектирования DTO
    //(DTO (Data Transfer Object) — это шаблон проектирования,
    //используемый для передачи данных между слоями приложения (например, между клиентом и сервером).)
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
    //readonly - запрещает редактировать переменную после создания и даёт понять другим программистом что этого делать не надо 
    private readonly ApplicationContext _context;

    private readonly ILogger<MyTaskServices> _logger;
    //CRUD операции для задач (Create, Read, Update, Delete)
    //Работа с БД через Entity Framework Core
    //Асинхронные операции
    public MyTaskServices(ApplicationContext context, ILogger<MyTaskServices> logger)
    {
        _context = context;//доступ к базе данных что бы читать и созранять
        _logger = logger;//журнал ошибок 
    }
    //Внедрение зависимостей (DB Context и Logger)
        public async Task<MyTaskResponse?> AddMyTaskAsync(CreateMyTaskRequest request)
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
                    TaskStatus = status
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


        //получение списка всех задач
    public async Task<List<MyTaskResponse>?> GetAllMyTaskAsync()
    {

        try
        {
            var mytasks = await _context.MyTasks.Include(x => x.TaskStatus).ToListAsync();

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


    //получение задачи по ID
    public async Task<MyTaskResponse?> GetMyTaskByIdAsync(Guid MyTaskId)
    {
        try
        {
            var mytask = await _context.MyTasks.Include(t => t.TaskStatus).FirstOrDefaultAsync(x => x.Id == MyTaskId);
            if(mytask == null)
            {
                // _logger.LogInformation($"");
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


    //обновление задачи 
    public async Task<MyTaskResponse?> UpdateMyTaskAsync(Guid MyTaskId, UpdateMyTaskRequest updateRequest)
    {
        try
        {
            var existingTask = await _context.MyTasks
                    .Include(t => t.TaskStatus)
                    .FirstOrDefaultAsync(x => x.Id == MyTaskId);
            if (existingTask == null)
            {
                // _logger.LogInformation($"");
                return null;
            }
            existingTask.Name = updateRequest.Name;
            existingTask.Description = updateRequest.Description;
            existingTask.Type = updateRequest.Type;
            existingTask.TaskStatus.Status = updateRequest.Status;

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

    //удаление задачи
    public Task<bool> DeleteMyTaskAsync(Guid MyTaskId)
    {
        var mytask = _context.MyTasks.Find(MyTaskId);
        if(mytask == null)
        {
            // _logger.LogInformation($"");
            return Task.FromResult(false);
        }
        try
        {
            _context.MyTasks.Remove(mytask);
            _context.SaveChanges();
            // _logger.LogInformation($"");
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Task.FromResult(false);
        }
    }
}
}
