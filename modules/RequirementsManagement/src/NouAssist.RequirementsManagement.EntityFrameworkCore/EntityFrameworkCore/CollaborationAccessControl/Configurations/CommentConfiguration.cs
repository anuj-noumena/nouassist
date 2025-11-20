using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Constants;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CollaborationDbProperties.DbTablePrefix + "Comments", 
                            CollaborationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ThreadId).IsRequired();
            builder.Property(x => x.AuthorId).IsRequired();
            builder.Property(x => x.CreationTime).IsRequired();
            builder.Property(x => x.IsEdited).IsRequired();
            builder.Property(x => x.LastEditTime);

            // Value Object Mapping
            builder.OwnsOne(x => x.Content, contentBuilder =>
            {
                contentBuilder.Property(c => c.Value)
                    .HasColumnName("Content")
                    .IsRequired()
                    .HasMaxLength(CollaborationConsts.MaxCommentContentLength);

                contentBuilder.Property(c => c.MentionedUserIds)
                    .HasColumnName("MentionedUserIds")
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(id => Guid.Parse(id))
                            .ToList());
            });

            // Indexing
            builder.HasIndex(x => x.ThreadId);
            builder.HasIndex(x => x.AuthorId);
            builder.HasIndex(x => x.CreationTime);
        }
    }
}
