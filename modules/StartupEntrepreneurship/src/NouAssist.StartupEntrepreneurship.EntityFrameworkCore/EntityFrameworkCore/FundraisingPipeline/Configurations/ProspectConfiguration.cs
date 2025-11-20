using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations
{
    public class ProspectConfiguration : IEntityTypeConfiguration<Prospect>
    {
        public void Configure(EntityTypeBuilder<Prospect> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                FundraisingPipelineDbProperties.DbTablePrefix + "Prospects",
                FundraisingPipelineDbProperties.DbSchema);

            // Primary Key (handled by AggregateRoot<Guid>)
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(FundraisingPipelineConsts.Prospect.MaxNameLength);

            builder.Property(x => x.Company)
                .IsRequired()
                .HasMaxLength(FundraisingPipelineConsts.Prospect.MaxCompanyLength);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(FundraisingPipelineConsts.Prospect.MaxTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(FundraisingPipelineConsts.Prospect.MaxDescriptionLength);

            builder.Property(x => x.FundraisingRoundId)
                .IsRequired(false);

            // Value Object Mappings

            // ContactInfo
            builder.OwnsOne(x => x.ContactInfo, contactBuilder =>
            {
                contactBuilder.Property(c => c.Email)
                    .HasColumnName("ContactEmail")
                    .IsRequired()
                    .HasMaxLength(FundraisingPipelineConsts.Contact.MaxEmailLength);

                contactBuilder.Property(c => c.Phone)
                    .HasColumnName("ContactPhone")
                    .HasMaxLength(FundraisingPipelineConsts.Contact.MaxPhoneLength);

                contactBuilder.Property(c => c.Address)
                    .HasColumnName("ContactAddress")
                    .HasMaxLength(FundraisingPipelineConsts.Contact.MaxAddressLength);
            });

            // PipelineStatus
            builder.OwnsOne(x => x.PipelineStatus, statusBuilder =>
            {
                statusBuilder.Property(s => s.Status)
                    .HasColumnName("Status")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                statusBuilder.Property(s => s.ChangedAt)
                    .HasColumnName("StatusChangedAt")
                    .IsRequired();

                statusBuilder.Property(s => s.DaysInCurrentStatus)
                    .HasColumnName("DaysInCurrentStatus")
                    .IsRequired();

                statusBuilder.Property(s => s.ConversionProbability)
                    .HasColumnName("ConversionProbability")
                    .IsRequired();
            });

            // ProspectScore
            builder.OwnsOne(x => x.Score, scoreBuilder =>
            {
                scoreBuilder.Property(s => s.EngagementScore)
                    .HasColumnName("EngagementScore")
                    .IsRequired();

                scoreBuilder.Property(s => s.FitScore)
                    .HasColumnName("FitScore")
                    .IsRequired();

                scoreBuilder.Property(s => s.TimelinessScore)
                    .HasColumnName("TimelinessScore")
                    .IsRequired();

                scoreBuilder.Property(s => s.OverallScore)
                    .HasColumnName("OverallScore")
                    .IsRequired();
            });

            // OpportunityForecastValue
            builder.OwnsOne(x => x.OpportunityForecast, forecastBuilder =>
            {
                forecastBuilder.Property(f => f.MinAmount)
                    .HasColumnName("ForecastMinAmount")
                    .HasColumnType("decimal(18,2)");

                forecastBuilder.Property(f => f.MaxAmount)
                    .HasColumnName("ForecastMaxAmount")
                    .HasColumnType("decimal(18,2)");

                forecastBuilder.Property(f => f.ExpectedAmount)
                    .HasColumnName("ForecastExpectedAmount")
                    .HasColumnType("decimal(18,2)");

                forecastBuilder.Property(f => f.Probability)
                    .HasColumnName("ForecastProbability");

                forecastBuilder.Property(f => f.ForecastedClosingDate)
                    .HasColumnName("ForecastedClosingDate");

                // Ignore calculated property from database mapping
                forecastBuilder.Ignore(f => f.WeightedAmount);
            });

            // Relationship Mapping (One-to-Many for interactions)
            builder.HasMany(x => x.Interactions)
                .WithOne()
                .HasForeignKey(i => i.ProspectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Company);
            builder.HasIndex("Status"); // Indexing the owned property
            builder.HasIndex(x => x.FundraisingRoundId);
        }
    }
}
