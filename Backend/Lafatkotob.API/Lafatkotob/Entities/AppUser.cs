using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Lafatkotob.Entities
{
    public class AppUser : IdentityUser 
    {
        public string Location { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime LastLogin { get; set; }
        public string ProfilePicture { get; set; }
        public string About { get; set; }
        public DateTime DTHDate { get; set; }

        public int Age => DateTime.Today.Year - DTHDate.Year - (DateTime.Today.DayOfYear < DTHDate.DayOfYear ? 1 : 0);

        public AppUser()
        {
            Books = new HashSet<Book>();
            UserPreferences = new HashSet<UserPreference>();
            BookPostLikes = new HashSet<BookPostLike>();
            BookPostComments = new HashSet<BookPostComment>();
            UserLikes = new HashSet<UserLike>();
            UserLiked = new HashSet<UserLike>();
            UserReviews = new HashSet<UserReview>();
            UserReviewed = new HashSet<UserReview>();
            UserEvents = new HashSet<UserEvent>();
            UserBadges = new HashSet<UserBadge>();
            ConversationsUsers = new HashSet<ConversationsUser>();
            MessagesSent = new HashSet<Message>();
            MessagesReceived = new HashSet<Message>();
            NotificationsUsers = new HashSet<NotificationUser>();
            Wishlists = null; 
        }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<UserPreference> UserPreferences { get; set; }
        public virtual History History { get; set; }
        public virtual ICollection<BookPostLike> BookPostLikes { get; set; }
        public virtual ICollection<BookPostComment> BookPostComments { get; set; }
        public virtual ICollection<UserLike> UserLikes { get; set; }
        public virtual ICollection<UserLike> UserLiked { get; set; }
        public virtual ICollection<UserReview> UserReviews { get; set; }
        public virtual ICollection<UserReview> UserReviewed { get; set; }
        public virtual ICollection<UserEvent> UserEvents { get; set; }
        public virtual ICollection<UserBadge> UserBadges { get; set; }
        public virtual ICollection<ConversationsUser> ConversationsUsers { get; set; }
        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesReceived { get; set; }
        public virtual ICollection<NotificationUser> NotificationsUsers { get; set; }
        public virtual Wishlist Wishlists { get; set; }
    }
}
