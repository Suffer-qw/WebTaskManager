namespace WebTaskManager.Model
{
    public class MyTaskModel
    {
        //Guid: Использование глобальных уникальных идентификаторов вместо int
        public Guid Id { get; set; }

        public required string Name { get; set; } 

        public required string Description { get; set; }

        public required string Type { get; set; }

        public Guid TaskStatusId { get; set; }

        //Нужно для быстрого поиска Status.name не делая дополнительный запрос 
        public required TaskStatusModel TaskStatus { get; set; } 

        public Guid UserId { get; set; }

        public UserProfileModel User { get; set; }
    }
}
