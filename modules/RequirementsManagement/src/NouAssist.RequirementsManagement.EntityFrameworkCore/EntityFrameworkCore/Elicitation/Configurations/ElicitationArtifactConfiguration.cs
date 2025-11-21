using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Newtonsoft.Json;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Configurations
{
    public class ElicitationArtifactConfiguration : IEntityTypeConfiguration<ElicitationArtifact>
    {
        public void Configure(EntityTypeBuilder<ElicitationArtifact> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ElicitationDbProperties.DbTablePrefix + "ElicitationArtifacts", 
                           ElicitationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Basic Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ElicitationConsts.MaxArtifactTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ElicitationConsts.MaxArtifactDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.SessionId)
                .IsRequired();

            builder.Property(x => x.CreatedByStakeholderId)
                .IsRequired();

            // Type-specific content as JSON columns
            builder.Property(x => x.StoryText)
                .HasConversion(
                    v => v != null ? JsonConvert.SerializeObject(v, Formatting.None) : null,
                    v => !string.IsNullOrEmpty(v) ? JsonConvert.DeserializeObject<StoryText>(v) : null)
                .HasColumnType("jsonb");

            builder.Property(x => x.UseCaseModel)
                .HasConversion(
                    v => v != null ? JsonConvert.SerializeObject(v, Formatting.None) : null,
                    v => !string.IsNullOrEmpty(v) ? JsonConvert.DeserializeObject<UseCaseModel>(v) : null)
                .HasColumnType("jsonb");

            builder.Property(x => x.EventDefinition)
                .HasConversion(
                    v => v != null ? JsonConvert.SerializeObject(v, Formatting.None) : null,
                    v => !string.IsNullOrEmpty(v) ? JsonConvert.DeserializeObject<EventDefinition>(v) : null)
                .HasColumnType("jsonb");

            // IdeaTags as JSON array
            builder.Property(x => x.IdeaTags)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v.ToList(), Formatting.None),
                    v => JsonConvert.DeserializeObject<List<IdeaTag>>(v))
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.Type)
                .HasDatabaseName("IX_ElicitationArtifacts_Type");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_ElicitationArtifacts_Status");

            builder.HasIndex(x => x.SessionId)
                .HasDatabaseName("IX_ElicitationArtifacts_SessionId");

            builder.HasIndex(x => x.CreatedByStakeholderId)
                .HasDatabaseName("IX_ElicitationArtifacts_CreatedByStakeholderId");

            // Composite indexes
            builder.HasIndex(x => new { x.Type, x.Status })
                .HasDatabaseName("IX_ElicitationArtifacts_Type_Status");

            builder.HasIndex(x => new { x.SessionId, x.Type })
                .HasDatabaseName("IX_ElicitationArtifacts_Session_Type");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(ElicitationArtifact.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
