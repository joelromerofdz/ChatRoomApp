namespace ChatRoomApp.Helpers.Interfaces
{
    public interface IChatRoomAppRepository
    {
        IMessageHelper MessageHelper { get; }
        IUserHelper UserHelper { get; }
    }
}
