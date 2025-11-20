using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Text.Json;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations
{
    public class PricingModelConfiguration : IEntityTypeConfiguration<PricingModel>
    {
        public void Configure(EntityTypeBuilder<PricingModel> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(BusinessModelManagementDbProperties.DbTablePrefix + "PricingModels",
                           BusinessModelManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.RevenueModelId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(BusinessModelManagementConsts.RevenueModel.MaxPricingModelNameLength);

            builder.Property(x => x.Strategy)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Value Object Configuration - PricingTiers (stored as JSON)
            builder.Property(x => x.PricingTiers)
                .HasColumnName("PricingTiersJson")
                .HasConversion(
                    v => JsonSerializer.Serialize(v.Select(pt => new
                    {
                        pt.Name,
                        pt.Price,
                        pt.Currency,
                        pt.Features,
                        pt.MaxUsers,
                        pt.BillingPeriod
                    }), (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<dynamic>>(v, (JsonSerializerOptions)null)
                        .Select(pt => new PricingTier(
                            pt.GetProperty("Name").GetString(),
                            pt.GetProperty("Price").GetDecimal(),
                            pt.GetProperty("Currency").GetString(),
                            pt.GetProperty("BillingPeriod").GetString(),
                            JsonSerializer.Deserialize<List<string>>(pt.GetProperty("Features").GetRawText()),
                            pt.TryGetProperty("MaxUsers", out var maxUsers) && maxUsers.ValueKind != JsonValueKind.Null 
                                ? maxUsers.GetInt32() : (int?)null))
                        .ToList())
                .HasMaxLength(4000);

            // Indexes
            builder.HasIndex(x => x.RevenueModelId);
            builder.HasIndex(x => new { x.RevenueModelId, x.Name }).IsUnique();
        }
    }
}
