using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Lafatkotob.Entities
{
    public class AppUsers:IdentityUser
    {
        public string location { get; set; }
        public DateTime dateJoined { get; set; }
        public DateTime lastLogin { get; set; }
        public string profilePicture { get; set; }
        public string about { get; set; }
        public DateTime DTHDate { get; set; }
        public int age => DateTime.Today.Year - DTHDate.Year;


        public AppUsers()
        {
            Books = new HashSet<Books>();
            UserPreferences = new HashSet<UserPreferences>();
            BookPostLikes = new HashSet<BookPostLikes>();
            BookPostComments = new HashSet<BookPostComments>();
            UserLikes = new HashSet<UserLikes>();
            UserLiked = new HashSet<UserLikes>();
            UserReviews = new HashSet<UserReviews>();
            UserReviewed = new HashSet<UserReviews>();
            UserEvent = new HashSet<UserEvent>();
            UserBadges = new HashSet<UserBadges>();
            ConversationsUser = new HashSet<ConversationsUser>();
            MessagesSent = new HashSet<Messages>();
            MessagesReceived = new HashSet<Messages>();
            NotificationsUsers = new HashSet<NotificationsUsers>();
        }

        public ICollection<Books> Books { get; set; }
        public ICollection<UserPreferences> UserPreferences { get; set; }
        public History History { get; set; }
        public ICollection<BookPostLikes> BookPostLikes { get; set; }
        public ICollection<BookPostComments> BookPostComments { get; set; }
        public ICollection<UserLikes> UserLikes { get; set; }
        public ICollection<UserLikes> UserLiked { get; set; }
        public ICollection<UserReviews> UserReviews { get; set; }
        public ICollection<UserReviews> UserReviewed { get; set; }
        public ICollection<UserEvent> UserEvent { get; set; }
        public ICollection<UserBadges> UserBadges { get; set; }
        public ICollection<ConversationsUser> ConversationsUser { get; set; }
        public ICollection<Messages> MessagesSent { get; set; }
        public ICollection<Messages> MessagesReceived { get; set; }
        public ICollection<NotificationsUsers> NotificationsUsers { get; set; }
        public Wishlists Wishlists { get; set; }
    }


}
