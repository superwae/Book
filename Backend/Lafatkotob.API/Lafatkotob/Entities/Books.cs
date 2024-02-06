using Lafatkotob.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace Lafatkotob.Entities
{
    public class Books
    {
        int id { get; set; }
        string title { get; set; }
        string author { get; set; }
        string? description { get; set; }
        string coverImage { get; set; }
        int UserID { get; set; }
        DateTime publicationDate { get; set; }
        string? ISBN { get; set; }
        int? pageCount { get; set; }
        string condition { get; set; }
        string status { get; set; }
    }
}
