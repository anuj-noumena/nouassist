using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for CustomerInterview aggregate
    /// </summary>
    public class CustomerInterviewConfiguration : IEntityTypeConfiguration<CustomerInterview>
    {
        public void Configure(EntityTypeBuilder<CustomerInterview> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "CustomerInterviews", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxInterviewTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ProductDiscoveryConsts.MaxInterviewDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.ScheduledDate)
                .IsRequired();

            builder.Property(x => x.CompletedDate)
                .IsRequired(false);

            builder.Property(x => x.InterviewerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.CustomerEmail)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.CustomerSegmentId)
                .IsRequired(false);

            // Configure InterviewGuide value object as owned type
            builder.OwnsOne(x => x.Guide, guideBuilder =>
            {
                guideBuilder.Property(g => g.Objective)
                    .HasColumnName("GuideObjective")
                    .HasMaxLength(ProductDiscoveryConsts.MaxInterviewDescriptionLength)
                    .IsRequired();

                guideBuilder.Property(g => g.EstimatedDuration)
                    .HasColumnName("GuideEstimatedDuration")
                    .IsRequired();

                // Store questions as JSON
                guideBuilder.Property(g => g.Questions)
                    .HasColumnName("GuideQuestions")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                        v => JsonSerializer.Deserialize<List<string>>(v, JsonSerializerOptions.Default) ?? new List<string>())
                    .HasColumnType("jsonb");
            });

            // Configure child entity relationships
            builder.HasMany(x => x.Transcripts)
                .WithOne()
                .HasForeignKey("InterviewId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Insights)
                .WithOne()
                .HasForeignKey("InterviewId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to CustomerSegment
            builder.HasOne<CustomerSegment>()
                .WithMany()
                .HasForeignKey(x => x.CustomerSegmentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(x => x.ScheduledDate);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.CustomerSegmentId);
            builder.HasIndex(x => x.Type);

            // Configure ExtraProperties (handled by convention)
            builder.ConfigureExtraProperties();

            // Multi-tenancy support
            builder.ConfigureByConvention();
        }
    }
}
