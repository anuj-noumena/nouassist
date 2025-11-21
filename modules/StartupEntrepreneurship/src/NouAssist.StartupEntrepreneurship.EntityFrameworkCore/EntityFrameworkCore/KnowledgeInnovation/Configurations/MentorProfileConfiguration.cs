using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Configurations
{
    public class MentorProfileConfiguration : IEntityTypeConfiguration<MentorProfile>
    {
        public void Configure(EntityTypeBuilder<MentorProfile> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(KnowledgeInnovationDbProperties.DbTablePrefix + "MentorProfiles",
                            KnowledgeInnovationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.MentorProfile.NameMaxLength);

            builder.Property(x => x.Bio)
                .IsRequired()
                .HasMaxLength(Constants.KnowledgeInnovationConsts.MentorProfile.BioMaxLength);

            builder.Property(x => x.ProfilePictureUrl)
                .HasMaxLength(Constants.KnowledgeInnovationConsts.DefaultLengths.Url);

            builder.Property(x => x.Expertise)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ExperienceYears)
                .IsRequired();

            // Tags (Collection of strings stored as JSON)
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                .HasColumnName("Tags");

            // Value Object Collection: NetworkRelationship (stored as JSON)
            builder.Property(x => x.Relationships)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<NetworkRelationship>>(v, (JsonSerializerOptions)null) ?? new List<NetworkRelationship>())
                .HasColumnName("Relationships");

            // Relationship Mapping
            builder.HasMany(x => x.AdviceRecords)
                .WithOne()
                .HasForeignKey(a => a.MentorProfileId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Expertise);
            builder.HasIndex(x => x.ExperienceYears);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
