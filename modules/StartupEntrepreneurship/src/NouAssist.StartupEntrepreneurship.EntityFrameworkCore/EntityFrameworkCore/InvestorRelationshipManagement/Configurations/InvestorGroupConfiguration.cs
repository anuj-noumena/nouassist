using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for InvestorGroup entity
    /// </summary>
    public class InvestorGroupConfiguration : IEntityTypeConfiguration<InvestorGroup>
    {
        public void Configure(EntityTypeBuilder<InvestorGroup> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(IRMDbProperties.DbTablePrefix + "InvestorGroups", IRMDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(IRMConsts.InvestorGroup.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(IRMConsts.InvestorGroup.MaxDescriptionLength);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // Store investor IDs as a JSON array
            builder.Property(x => x.InvestorIds)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new List<Guid>()
                )
                .HasColumnName("InvestorIds")
                .HasColumnType("jsonb");

            // Indexes
            builder.HasIndex(x => x.Name)
                .HasDatabaseName("IX_InvestorGroups_Name")
                .IsUnique();

            builder.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_InvestorGroups_IsActive");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(InvestorGroup.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
