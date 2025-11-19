using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Collaboration.Comments.Entities;

namespace NouAssist.Collaboration.EntityFrameworkCore.Comments.EntityFrameworkCore.Configurations
{
    public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
    {
        public void Configure(EntityTypeBuilder<CommentReaction> builder)
        {
            builder.ToTable(CollaborationDbProperties.DbTablePrefix + "CommentReactions", CollaborationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CommentId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Emoji)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.CommentId);
            builder.HasIndex(x => new { x.CommentId, x.UserId, x.Emoji }).IsUnique();
        }
    }
}
