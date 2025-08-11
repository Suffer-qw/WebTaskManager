using WebTaskManager.Contracts;

namespace WebTaskManager.Interface
{
    public interface IMyTaskService
    {
        //интерфейс сервиса, который определяет контракт для операций CRUD (Create, Read, Update, Delete) с задачами. Он абстрагирует работу с данными, что позволяет:
        //Легко менять реализацию(например, переход с SQLite на SQL Server)
        //Упрощать тестирование
        //Cоблюдать принцип инверсии зависимостей

        //Generic Task<>: Все методы асинхронные (async/await), что критично для веб-приложений
        //Позволяет обрабатывать больше запросов одновременно
        //Особенно важно для операций ввода-вывода(БД, файлы, сетевые запросы)

        Task<MyTaskResponse?> AddMyTaskAsync(CreateMyTaskRequest request, Guid userId);
        Task<MyTaskResponse?> GetMyTaskByIdAsync(Guid MyTaskId, Guid userId);
        Task<List<MyTaskResponse>?> GetAllMyTaskAsync(Guid userId);
        Task<MyTaskResponse?> UpdateMyTaskAsync(Guid MyTaskId, UpdateMyTaskRequest updateRequest, Guid userId);
        Task<bool> DeleteMyTaskAsync(Guid MyTaskId, Guid userId);

    }
}
