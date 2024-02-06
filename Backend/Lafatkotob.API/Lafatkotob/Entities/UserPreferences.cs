namespace Lafatkotob.Entities
{
    public class UserPreferences
    {
        int id { get; set; }
        int userID { get; set; }
        int genreID { get; set; }
        string? preferredAuthor { get; set; }
    }
}
