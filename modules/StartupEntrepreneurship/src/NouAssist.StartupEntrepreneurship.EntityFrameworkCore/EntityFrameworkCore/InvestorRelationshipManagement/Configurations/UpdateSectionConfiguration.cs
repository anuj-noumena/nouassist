using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for UpdateSection entity
    /// </summary>
    public class UpdateSectionConfiguration : IEntityTypeConfiguration<UpdateSection>
    {
        public void Configure(EntityTypeBuilder<UpdateSection> builder)
        {
            builder.ToTable(IRMDbProperties.DbTablePrefix + "UpdateSections", IRMDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UpdateId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.IsVisible)
                .HasDefaultValue(true);

            // Value Object Mapping - UpdateContent
            builder.OwnsOne(x => x.Content, contentBuilder =>
            {
                contentBuilder.Property(c => c.Content)
                    .IsRequired()
                    .HasColumnName("ContentBody")
                    .HasMaxLength(IRMConsts.InvestorUpdate.MaxContentLength);

                contentBuilder.Property(c => c.ContentType)
                    .IsRequired()
                    .HasColumnName("ContentType")
                    .HasMaxLength(50)
                    .HasDefaultValue("text/html");

                contentBuilder.Property(c => c.Metadata)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new Dictionary<string, object>()
                    )
                    .HasColumnName("ContentMetadata")
                    .HasColumnType("jsonb");
            });

            // Indexes
            builder.HasIndex(x => x.UpdateId)
                .HasDatabaseName("IX_UpdateSections_UpdateId");

            builder.HasIndex(x => new { x.UpdateId, x.Order })
                .HasDatabaseName("IX_UpdateSections_UpdateId_Order");
        }
    }
}
