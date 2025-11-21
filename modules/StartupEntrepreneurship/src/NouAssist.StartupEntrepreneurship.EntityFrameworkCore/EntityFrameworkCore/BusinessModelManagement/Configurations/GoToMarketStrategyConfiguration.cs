using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Text.Json;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations
{
    public class GoToMarketStrategyConfiguration : IEntityTypeConfiguration<GoToMarketStrategy>
    {
        public void Configure(EntityTypeBuilder<GoToMarketStrategy> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(BusinessModelManagementDbProperties.DbTablePrefix + "GoToMarketStrategies",
                           BusinessModelManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.RevenueModelId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(BusinessModelManagementConsts.GoToMarket.MaxStrategyNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.TargetMarket)
                .HasMaxLength(500);

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate);

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Value Object Configuration - GtmPlanElements (stored as JSON)
            builder.Property(x => x.PlanElements)
                .HasColumnName("PlanElementsJson")
                .HasConversion(
                    v => JsonSerializer.Serialize(v.Select(pe => new
                    {
                        pe.ChannelType,
                        pe.ChannelName,
                        pe.Description,
                        pe.TargetAudience,
                        pe.EstimatedCost,
                        pe.Currency,
                        pe.Priority,
                        pe.StartDate,
                        pe.EndDate,
                        pe.KeyMetrics
                    }), (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<dynamic>>(v, (JsonSerializerOptions)null)
                        .Select(pe => new GtmPlanElement(
                            Enum.Parse<GtmChannelType>(pe.GetProperty("ChannelType").GetString()),
                            pe.GetProperty("ChannelName").GetString(),
                            pe.GetProperty("Description").GetString(),
                            pe.GetProperty("TargetAudience").GetString(),
                            pe.GetProperty("EstimatedCost").GetDecimal(),
                            pe.GetProperty("Currency").GetString(),
                            pe.GetProperty("Priority").GetInt32(),
                            pe.GetProperty("StartDate").GetDateTime(),
                            pe.TryGetProperty("EndDate", out var endDate) && endDate.ValueKind != JsonValueKind.Null 
                                ? endDate.GetDateTime() : (DateTime?)null,
                            JsonSerializer.Deserialize<List<string>>(pe.GetProperty("KeyMetrics").GetRawText())))
                        .ToList())
                .HasMaxLength(6000);

            // Indexes
            builder.HasIndex(x => x.RevenueModelId);
            builder.HasIndex(x => new { x.RevenueModelId, x.Name }).IsUnique();
        }
    }
}
