using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CollaborationHub.Resources.Aggregates;
using NouAssist.CollaborationHub.Resources.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CollaborationHub.Resources.EntityFrameworkCore.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ResourcesDbProperties.DbTablePrefix + "Books", 
                           ResourcesDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId).IsRequired();
            builder.Property(x => x.ResourceId).IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Subtitle)
                .HasMaxLength(500);

            builder.Property(x => x.Description)
                .HasMaxLength(4000);

            builder.Property(x => x.Publisher)
                .HasMaxLength(256);

            builder.Property(x => x.PublishedDate)
                .HasMaxLength(50);

            builder.Property(x => x.Edition)
                .HasMaxLength(100);

            builder.Property(x => x.Language)
                .HasMaxLength(10);

            builder.Property(x => x.Isbn10)
                .HasMaxLength(20);

            builder.Property(x => x.Isbn13)
                .HasMaxLength(20);

            builder.Property(x => x.Asin)
                .HasMaxLength(50);

            builder.Property(x => x.Doi)
                .HasMaxLength(100);

            builder.Property(x => x.SeriesName)
                .HasMaxLength(256);

            builder.Property(x => x.CollectionName)
                .HasMaxLength(256);

            builder.Property(x => x.UserReview)
                .HasMaxLength(4000);

            builder.Property(x => x.UserNotes)
                .HasColumnType("text");

            builder.Property(x => x.ReadingStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Format)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Source)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // List columns
            builder.Property(x => x.Authors)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .HasMaxLength(1000);

            builder.Property(x => x.Categories)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .HasMaxLength(1000);

            builder.Property(x => x.Tags)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .HasMaxLength(1000);

            // Value Object: GoogleBookData
            builder.OwnsOne(x => x.GoogleBookData, google =>
            {
                google.Property(g => g.ApiTitle)
                    .HasColumnName("GoogleApiTitle")
                    .HasMaxLength(500);

                google.Property(g => g.Publisher)
                    .HasColumnName("GooglePublisher")
                    .HasMaxLength(256);

                google.Property(g => g.PublishedDate)
                    .HasColumnName("GooglePublishedDate")
                    .HasMaxLength(50);

                google.Property(g => g.Description)
                    .HasColumnName("GoogleDescription")
                    .HasMaxLength(4000);

                google.Property(g => g.PageCount)
                    .HasColumnName("GooglePageCount");

                google.Property(g => g.Isbn10)
                    .HasColumnName("GoogleIsbn10")
                    .HasMaxLength(20);

                google.Property(g => g.Isbn13)
                    .HasColumnName("GoogleIsbn13")
                    .HasMaxLength(20);

                google.Property(g => g.PreviewLink)
                    .HasColumnName("GooglePreviewLink")
                    .HasMaxLength(2048);

                google.Property(g => g.InfoLink)
                    .HasColumnName("GoogleInfoLink")
                    .HasMaxLength(2048);

                google.Property(g => g.ThumbnailUrl)
                    .HasColumnName("GoogleThumbnailUrl")
                    .HasMaxLength(2048);

                google.Property(g => g.SmallThumbnailUrl)
                    .HasColumnName("GoogleSmallThumbnailUrl")
                    .HasMaxLength(2048);

                google.Property(g => g.Language)
                    .HasColumnName("GoogleLanguage")
                    .HasMaxLength(10);

                google.Property(g => g.AverageRating)
                    .HasColumnName("GoogleAverageRating");

                google.Property(g => g.RatingsCount)
                    .HasColumnName("GoogleRatingsCount");

                google.Property(g => g.MaturityRating)
                    .HasColumnName("GoogleMaturityRating")
                    .HasMaxLength(50);

                google.Property(g => g.FetchedAt)
                    .HasColumnName("GoogleFetchedAt");

                google.Property(g => g.ApiAuthors)
                    .HasColumnName("GoogleApiAuthors")
                    .HasConversion(
                        v => string.Join(';', v),
                        v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()
                    )
                    .HasMaxLength(1000);

                google.Property(g => g.ApiCategories)
                    .HasColumnName("GoogleApiCategories")
                    .HasConversion(
                        v => string.Join(';', v),
                        v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()
                    )
                    .HasMaxLength(1000);

                google.Property(g => g.RawData)
                    .HasColumnName("GoogleRawData")
                    .HasColumnType("jsonb");
            });

            // JSON columns
            builder.Property(x => x.CustomFields)
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.ReadingStatus);
            builder.HasIndex(x => x.Isbn13);
            builder.HasIndex(x => x.Isbn10);
            builder.HasIndex(x => x.SeriesName);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
