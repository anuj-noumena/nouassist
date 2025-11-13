using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.Reviews.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Reviews.Configurations
{
    public class ReviewItemConfiguration : IEntityTypeConfiguration<ReviewItem>
    {
        public void Configure(EntityTypeBuilder<ReviewItem> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ReviewsDbProperties.DbTablePrefix + "ReviewItems",
                           ReviewsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ReviewId)
                .IsRequired();

            builder.Property(x => x.EntityId)
                .IsRequired();

            builder.Property(x => x.ItemType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ItemTitle)
                .HasMaxLength(200);

            builder.Property(x => x.ReviewNotes)
                .HasMaxLength(2000);

            builder.Property(x => x.IsReviewed)
                .IsRequired();

            builder.Property(x => x.ReviewedAt);

            builder.Property(x => x.Decision)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ActionTaken)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.ReviewId);
            builder.HasIndex(x => x.EntityId);
            builder.HasIndex(x => x.ItemType);
            builder.HasIndex(x => x.IsReviewed);
            builder.HasIndex(x => new { x.ReviewId, x.IsReviewed });
        }
    }
}
