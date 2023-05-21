namespace ChatRoomApp.Base
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public EntityBase()
        {
            this.CreatedDate = DateTime.Now;
        }
    }
}
