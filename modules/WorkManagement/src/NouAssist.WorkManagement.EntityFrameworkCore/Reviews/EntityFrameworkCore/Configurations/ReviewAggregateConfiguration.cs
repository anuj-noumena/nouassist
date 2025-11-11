using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.Reviews.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.Reviews.EntityFrameworkCore.Configurations
{
    public class ReviewAggregateConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ReviewsDbProperties.DbTablePrefix + "Reviews",
                           ReviewsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.ReviewType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.PeriodStart);
            builder.Property(x => x.PeriodEnd);

            builder.Property(x => x.WhatWentWell)
                .HasMaxLength(2000);

            builder.Property(x => x.WhatNeedsAttention)
                .HasMaxLength(2000);

            builder.Property(x => x.Insights)
                .HasMaxLength(2000);

            builder.Property(x => x.LessonsLearned)
                .HasMaxLength(2000);

            builder.Property(x => x.NextPeriodFocus)
                .HasMaxLength(2000);

            builder.Property(x => x.CompletenessScore);
            builder.Property(x => x.SatisfactionScore);
            builder.Property(x => x.AlignmentScore);
            builder.Property(x => x.EnergyLevel);
            builder.Property(x => x.TimeSpentMinutes);

            builder.Property(x => x.IsComplete)
                .IsRequired();

            builder.Property(x => x.CompletedAt);
            builder.Property(x => x.NextReviewDue);

            // Action Items as JSON (Value Objects)
            builder.OwnsMany(x => x.ActionItems, actionItemBuilder =>
            {
                actionItemBuilder.ToJson();
                actionItemBuilder.Property(a => a.Id).IsRequired();
                actionItemBuilder.Property(a => a.Description).IsRequired().HasMaxLength(500);
                actionItemBuilder.Property(a => a.AssignedTo).HasMaxLength(200);
                actionItemBuilder.Property(a => a.DueBy);
                actionItemBuilder.Property(a => a.IsCompleted).IsRequired();
                actionItemBuilder.Property(a => a.CompletedAt);
            });

            // Relationships
            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey("ReviewId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.ReviewType);
            builder.HasIndex(x => x.IsComplete);
            builder.HasIndex(x => new { x.OwnerId, x.ReviewType, x.IsComplete });
            builder.HasIndex(x => x.CompletedAt);
            builder.HasIndex(x => x.NextReviewDue);
            builder.HasIndex(x => x.TenantId);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
