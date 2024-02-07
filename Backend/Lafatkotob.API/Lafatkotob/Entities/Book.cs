using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lafatkotob.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; } 
        public string CoverImage { get; set; }

        [ForeignKey(nameof(AppUser))]
        public string UserId { get; set; } 

        public DateTime PublicationDate { get; set; }
        public string ISBN { get; set; } 
        public int? PageCount { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<BookGenre> BookGenres { get; set; }
        public virtual ICollection<BookPostLike> BookPostLikes { get; set; }
        public virtual ICollection<BookPostComment> BookPostComments { get; set; }

        public Book()
        {
            BookGenres = new HashSet<BookGenre>();
            BookPostLikes = new HashSet<BookPostLike>();
            BookPostComments = new HashSet<BookPostComment>();
        }
    }
}
