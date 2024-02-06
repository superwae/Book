namespace Lafatkotob.Entities
{
    public class BookPostComments
    {
       int id {  get; set; }
       int bookID { get; set; }
       int userID { get; set; }
      string commentText { get; set; }
      DateTime dateCommented { get; set; }

    }
}
