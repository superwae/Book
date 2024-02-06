namespace Lafatkotob.Entities
{
    public class Messages
    {
        int conversationID { get; set; }
        int id { get; set; }
        int senderUserID { get; set; }
        int receiveUserID { get; set; }
        string messageText { get; set; }
        DataTime dateSent { get; set; }
        int isReceived { get; set; }
        int isRead { get; set; }
        int isDeletedBySender { get; set; }
        int isDeletedByReceiver { get; set; }
    }
}
