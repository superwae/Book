using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Lafatkotob.Entities;

namespace Lafatkotob.Configuration
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversations");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.LastMessageDate).IsRequired();

            // Configuring the one-to-many relationship with Message
            builder.HasMany(c => c.Messages)
                   .WithOne(m => m.Conversation)
                   .HasForeignKey(m => m.ConversationId)
                   .OnDelete(DeleteBehavior.Cascade); // Adjust the delete behavior based on your domain requirements

            // Assuming ConversationsUser is an entity that represents the many-to-many relationship between Conversations and Users
            // If ConversationsUser entity exists and is defined as such, further configuration might be required to map this relationship explicitly.
            // However, based on the given information, detailed configuration for ConversationsUser is not provided here.
        }
    }
}
