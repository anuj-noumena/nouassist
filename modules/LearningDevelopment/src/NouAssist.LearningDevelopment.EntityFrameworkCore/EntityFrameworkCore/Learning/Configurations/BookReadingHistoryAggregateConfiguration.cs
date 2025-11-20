using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Learning.Configurations
{
    public class BookReadingHistoryAggregateConfiguration : IEntityTypeConfiguration<BookReadingHistory>
    {
        public void Configure(EntityTypeBuilder<BookReadingHistory> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                LearningDevelopmentDbProperties.DbTablePrefix + "BookReadingHistories",
                LearningDevelopmentDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName(nameof(BookReadingHistory.UserId));

            builder.Property(x => x.TopicId)
                .HasColumnName(nameof(BookReadingHistory.TopicId));

            builder.Property(x => x.BookId)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName(nameof(BookReadingHistory.BookId));

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(512)
                .HasColumnName(nameof(BookReadingHistory.Title));

            builder.Property(x => x.Author)
                .HasMaxLength(256)
                .HasColumnName(nameof(BookReadingHistory.Author));

            builder.Property(x => x.CoverImageUrl)
                .HasMaxLength(2048)
                .HasColumnName(nameof(BookReadingHistory.CoverImageUrl));

            builder.Property(x => x.LastReadPage)
                .IsRequired()
                .HasColumnName(nameof(BookReadingHistory.LastReadPage));

            builder.Property(x => x.TotalPages)
                .HasColumnName(nameof(BookReadingHistory.TotalPages));

            builder.Property(x => x.ProgressPercentage)
                .HasColumnName(nameof(BookReadingHistory.ProgressPercentage));

            builder.Property(x => x.Timestamp)
                .IsRequired()
                .HasColumnName(nameof(BookReadingHistory.Timestamp));

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_Learning_BookReadingHistories_UserId");

            builder.HasIndex(x => x.TopicId)
                .HasDatabaseName("IX_Learning_BookReadingHistories_TopicId");

            builder.HasIndex(x => new { x.UserId, x.BookId })
                .IsUnique()
                .HasDatabaseName("IX_Learning_BookReadingHistories_UserId_BookId");

            builder.HasIndex(x => x.Timestamp)
                .HasDatabaseName("IX_Learning_BookReadingHistories_Timestamp");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(BookReadingHistory.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40)
                .HasColumnName(nameof(BookReadingHistory.ConcurrencyStamp));
        }
    }
}
