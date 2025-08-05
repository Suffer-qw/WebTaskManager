namespace WebTaskManager.Model
{
    public class MyTaskModel
    {
        //Guid: Использование глобальных уникальных идентификаторов вместо int
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public Guid TaskStatusId { get; set; }

        //Нужно для быстрого поиска Status.name не делая дополнительный запрос 
        public TaskStatusModel TaskStatus { get; set; } 
    }
}
