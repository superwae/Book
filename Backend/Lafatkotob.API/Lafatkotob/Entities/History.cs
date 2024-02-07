using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lafatkotob.Entities
{
    public class History
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AppUser")]
        public string UserId { get; set; } 

        [ForeignKey("Book")]
        public int BookId { get; set; }

        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string State { get; set; }

        [ForeignKey("PartnerAppUser")]
        public string PartnerUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual AppUser PartnerAppUser { get; set; }
        public virtual Book Book { get; set; }
    }
}
