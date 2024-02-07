using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lafatkotob.Entities
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        public DateTime LastMessageDate { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public Conversation()
        {
            Messages = new HashSet<Message>();
        }
    }
}
