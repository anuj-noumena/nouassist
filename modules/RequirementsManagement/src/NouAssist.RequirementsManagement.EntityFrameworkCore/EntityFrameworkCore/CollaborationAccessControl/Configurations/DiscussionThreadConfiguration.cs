using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Constants;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Configurations
{
    public class DiscussionThreadConfiguration : IEntityTypeConfiguration<DiscussionThread>
    {
        public void Configure(EntityTypeBuilder<DiscussionThread> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CollaborationDbProperties.DbTablePrefix + "DiscussionThreads", 
                            CollaborationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.DiscussionBoardId).IsRequired();
            builder.Property(x => x.CreatorId).IsRequired();
            builder.Property(x => x.CreationTime).IsRequired();
            builder.Property(x => x.IsClosed).IsRequired();
            builder.Property(x => x.ClosedTime);

            // Value Object Mapping
            builder.OwnsOne(x => x.Subject, subjectBuilder =>
            {
                subjectBuilder.Property(s => s.Value)
                    .HasColumnName("Subject")
                    .IsRequired()
                    .HasMaxLength(CollaborationConsts.MaxThreadSubjectLength);
            });

            // Relationships
            builder.HasMany(x => x.Comments)
                .WithOne()
                .HasForeignKey(c => c.ThreadId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.DiscussionBoardId);
            builder.HasIndex(x => x.CreatorId);
            builder.HasIndex(x => x.IsClosed);
            builder.HasIndex(x => x.CreationTime);
        }
    }
}
