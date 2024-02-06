namespace Lafatkotob.Entities
{
    public class History
    {
        int id { get; set; }
        int userID { get; set; }
        int bookID { get; set; }
        string date { get; set; }
        string type { get; set; }
        string state { get; set; }
        int partnerUserID { get; set; }
    }
}
