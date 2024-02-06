namespace Lafatkotob.Entities
{
    public class UserReviews
    {
        int id { get; set; }
        int reviewedUserID { get; set; }
        int reviewingUserID { get; set; }
        string? reviewText { get; set; }
        DateTime dateReviewed { get; set; }
        int rating { get; set; }

    }
}
