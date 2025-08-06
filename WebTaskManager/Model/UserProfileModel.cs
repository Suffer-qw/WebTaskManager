namespace WebTaskManager.Model
{
    public class UserProfileModel
    {
        //Guid: Использование глобальных уникальных идентификаторов вместо int
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }
    }
}
