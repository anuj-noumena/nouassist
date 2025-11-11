using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Collaboration.Comments.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Collaboration.Comments.EntityFrameworkCore.Configurations
{
    public class CommentAggregateConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(CommentsDbProperties.DbTablePrefix + "Comments", CommentsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AuthorId)
                .IsRequired();

            builder.Property(x => x.ParentCommentId);

            builder.Property(x => x.EntityType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.EntityId)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.IsPinned)
                .IsRequired();

            builder.Property(x => x.IsEdited)
                .IsRequired();

            builder.Property(x => x.EditedAt);

            builder.Property(x => x.IsDeleted)
                .IsRequired();

            builder.Property(x => x.DeletedAt);

            builder.Property(x => x.ReactionsCount)
                .IsRequired();

            builder.Property(x => x.RepliesCount)
                .IsRequired();

            // Collection: MentionedUserIds (stored as JSON array)
            builder.OwnsMany(x => x.MentionedUserIds, muidBuilder =>
            {
                muidBuilder.ToJson();
            });

            // Relationship with CommentReaction
            builder.HasMany(x => x.Reactions)
                .WithOne()
                .HasForeignKey(r => r.CommentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.AuthorId);
            builder.HasIndex(x => new { x.EntityType, x.EntityId });
            builder.HasIndex(x => x.ParentCommentId);
            builder.HasIndex(x => new { x.EntityType, x.EntityId, x.ParentCommentId });

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
