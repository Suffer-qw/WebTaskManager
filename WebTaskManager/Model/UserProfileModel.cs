namespace WebTaskManager.Model
{
    public class UserProfileModel
    {
        //Guid: Использование глобальных уникальных идентификаторов вместо int
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Key { get; set; }
    }
}
