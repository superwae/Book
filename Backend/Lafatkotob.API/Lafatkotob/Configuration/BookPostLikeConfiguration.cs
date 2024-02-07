﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Lafatkotob.Entities;

namespace Lafatkotob.Configuration
{
    public class BookPostLikeConfiguration : IEntityTypeConfiguration<BookPostLike>
    {
        public void Configure(EntityTypeBuilder<BookPostLike> builder)
        {

            builder.HasKey(bpl => bpl.Id);

            builder.Property(bpl => bpl.DateLiked).IsRequired();

            // Configuring the relationship with Book
            builder.HasOne(bpl => bpl.Book)
                   .WithMany(b => b.BookPostLikes)
                   .HasForeignKey(bpl => bpl.BookId)
                   .OnDelete(DeleteBehavior.Cascade); 

            // Configuring the relationship with AppUser
            builder.HasOne(bpl => bpl.AppUser)
                   .WithMany(au => au.BookPostLikes)
                   .HasForeignKey(bpl => bpl.UserId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
