using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations
{
    public class PitchPresentationConfiguration : IEntityTypeConfiguration<PitchPresentation>
    {
        public void Configure(EntityTypeBuilder<PitchPresentation> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                FundraisingPipelineDbProperties.DbTablePrefix + "PitchPresentations", 
                FundraisingPipelineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.PitchDeckId)
                .IsRequired();

            builder.Property(x => x.VersionId)
                .IsRequired();

            builder.Property(x => x.ProspectId)
                .IsRequired();

            // Value Object Mapping - PresentationDetails
            builder.OwnsOne(x => x.Details, detailsBuilder =>
            {
                detailsBuilder.Property(d => d.PresentationDate)
                    .HasColumnName("PresentationDate")
                    .IsRequired();

                detailsBuilder.Property(d => d.Duration)
                    .HasColumnName("Duration")
                    .IsRequired();

                detailsBuilder.Property(d => d.Location)
                    .HasColumnName("Location")
                    .HasMaxLength(500);

                detailsBuilder.Property(d => d.MeetingFormat)
                    .HasColumnName("MeetingFormat")
                    .IsRequired()
                    .HasMaxLength(50);

                // For the Attendees collection, we'll use JSON serialization
                detailsBuilder.Property(d => d.Attendees)
                    .HasColumnName("Attendees")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                    .HasColumnType("jsonb");
            });

            // Indexing
            builder.HasIndex(x => x.PitchDeckId);
            builder.HasIndex(x => x.VersionId);
            builder.HasIndex(x => x.ProspectId);
            builder.HasIndex("PresentationDate"); // Index the owned property
            builder.HasIndex(x => x.CreationTime);
        }
    }
}
