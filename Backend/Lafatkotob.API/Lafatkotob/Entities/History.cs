using System.ComponentModel.DataAnnotations.Schema;

namespace Lafatkotob.Entities
{
    public class History
    {
        int id { get; set; }

        [ForeignKey("User")]
        int userID { get; set; }
        [ForeignKey("Books")]
        int bookID { get; set; }
        string date { get; set; }
        string type { get; set; }
        string state { get; set; }
        int partnerUserID { get; set; }
    }
}
