using System.ComponentModel.DataAnnotations;

namespace Lafatkotob.Entities
{
    public class BooksInWishlists
    {
        
        int id { get; set; }
        string title { get; set; }
        string author { get; set; }

        string? ISBN { get; set; }
    }
}

