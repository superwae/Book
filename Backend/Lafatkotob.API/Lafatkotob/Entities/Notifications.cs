namespace Lafatkotob.Entities
{
    public class Notifications
    {
        int id {  get; set; }
        string message {  get; set; }
        DateTime dateSent { get; set; }
        bool isRead { get; set; }

    }
}
