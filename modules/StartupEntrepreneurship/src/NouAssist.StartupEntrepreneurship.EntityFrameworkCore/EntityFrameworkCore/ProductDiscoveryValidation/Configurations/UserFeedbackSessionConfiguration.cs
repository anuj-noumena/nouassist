using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for UserFeedbackSession aggregate
    /// </summary>
    public class UserFeedbackSessionConfiguration : IEntityTypeConfiguration<UserFeedbackSession>
    {
        public void Configure(EntityTypeBuilder<UserFeedbackSession> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "UserFeedbackSessions", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxInterviewTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxInterviewDescriptionLength);

            builder.Property(x => x.RelatedPrototypeId)
                .IsRequired(false);

            builder.Property(x => x.PrototypeVersionId)
                .IsRequired(false);

            builder.Property(x => x.TestScenario)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxValidationNotesLength);

            builder.Property(x => x.ScheduledDate)
                .IsRequired();

            builder.Property(x => x.CompletedDate)
                .IsRequired(false);

            builder.Property(x => x.ParticipantEmail)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.ParticipantProfile)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Configure child entity relationships
            builder.HasMany(x => x.FeedbackData)
                .WithOne()
                .HasForeignKey("FeedbackSessionId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign keys to SolutionPrototype and PrototypeVersion
            builder.HasOne<SolutionPrototype>()
                .WithMany()
                .HasForeignKey(x => x.RelatedPrototypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<PrototypeVersion>()
                .WithMany()
                .HasForeignKey(x => x.PrototypeVersionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(x => x.ScheduledDate);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.RelatedPrototypeId);
            builder.HasIndex(x => x.PrototypeVersionId);
            builder.HasIndex(x => x.ParticipantEmail);

            // Configure ExtraProperties and multi-tenancy
            builder.ConfigureExtraProperties();
            builder.ConfigureByConvention();
        }
    }
}
