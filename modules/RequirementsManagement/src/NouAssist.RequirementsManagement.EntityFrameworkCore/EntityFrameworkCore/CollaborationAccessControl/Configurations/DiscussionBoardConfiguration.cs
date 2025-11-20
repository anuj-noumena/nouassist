using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Configurations
{
    public class DiscussionBoardConfiguration : IEntityTypeConfiguration<DiscussionBoard>
    {
        public void Configure(EntityTypeBuilder<DiscussionBoard> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CollaborationDbProperties.DbTablePrefix + "DiscussionBoards", 
                            CollaborationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

            // Value Object Mapping - TargetResource
            builder.OwnsOne(x => x.Resource, resourceBuilder =>
            {
                resourceBuilder.Property(r => r.Type)
                    .HasColumnName("ResourceType")
                    .HasConversion<string>()
                    .IsRequired()
                    .HasMaxLength(50);

                resourceBuilder.Property(r => r.Id)
                    .HasColumnName("ResourceId");
            });

            // Relationships
            builder.HasMany(x => x.Threads)
                .WithOne()
                .HasForeignKey(t => t.DiscussionBoardId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex("ResourceType", "ResourceId").IsUnique();
        }
    }
}
