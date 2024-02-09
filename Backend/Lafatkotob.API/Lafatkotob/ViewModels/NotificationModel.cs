namespace Lafatkotob.ViewModels
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
        public bool IsRead { get; set; }
    }
}
