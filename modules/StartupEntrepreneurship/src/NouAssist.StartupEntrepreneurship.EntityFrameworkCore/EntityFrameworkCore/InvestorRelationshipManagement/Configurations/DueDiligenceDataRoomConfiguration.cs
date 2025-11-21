using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for DueDiligenceDataRoom aggregate
    /// </summary>
    public class DueDiligenceDataRoomConfiguration : IEntityTypeConfiguration<DueDiligenceDataRoom>
    {
        public void Configure(EntityTypeBuilder<DueDiligenceDataRoom> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(IRMDbProperties.DbTablePrefix + "DueDiligenceDataRooms", IRMDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(IRMConsts.DueDiligenceDataRoom.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(IRMConsts.DueDiligenceDataRoom.MaxDescriptionLength);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.RequirePasswordAccess)
                .HasDefaultValue(false);

            builder.Property(x => x.AccessPassword)
                .HasMaxLength(200);

            // Store investor access as JSON dictionary
            builder.Property(x => x.InvestorAccess)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(
                        v.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value.ToString()), 
                        (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions)null)?
                        .ToDictionary(kv => Guid.Parse(kv.Key), kv => Enum.Parse<AccessLevel>(kv.Value)) ?? 
                        new Dictionary<Guid, AccessLevel>()
                )
                .HasColumnName("InvestorAccess")
                .HasColumnType("jsonb");

            // One-to-Many Relationships within aggregate boundary
            builder.HasMany<SecureDocument>()
                .WithOne()
                .HasForeignKey("DataRoomId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Name)
                .HasDatabaseName("IX_DueDiligenceDataRooms_Name");

            builder.HasIndex(x => x.IsActive)
                .HasDatabaseName("IX_DueDiligenceDataRooms_IsActive");

            builder.HasIndex(x => x.ExpiryDate)
                .HasDatabaseName("IX_DueDiligenceDataRooms_ExpiryDate");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(DueDiligenceDataRoom.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();
        }
    }
}
