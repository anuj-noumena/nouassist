using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Text.Json;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations
{
    public class RevenueScenarioConfiguration : IEntityTypeConfiguration<RevenueScenario>
    {
        public void Configure(EntityTypeBuilder<RevenueScenario> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(BusinessModelManagementDbProperties.DbTablePrefix + "RevenueScenarios",
                           BusinessModelManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.RevenueModelId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(BusinessModelManagementConsts.RevenueModel.MaxScenarioNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(BusinessModelManagementConsts.RevenueModel.MaxScenarioDescriptionLength);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            builder.Property(x => x.LastSimulatedAt);

            // Value Object Configuration - ScenarioParameters
            builder.OwnsOne(x => x.Parameters, parametersBuilder =>
            {
                parametersBuilder.Property(p => p.CustomerCount)
                    .HasColumnName("CustomerCount")
                    .IsRequired();

                parametersBuilder.Property(p => p.AverageRevenuePerUser)
                    .HasColumnName("AverageRevenuePerUser")
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                parametersBuilder.Property(p => p.ChurnRate)
                    .HasColumnName("ChurnRate")
                    .IsRequired();

                parametersBuilder.Property(p => p.GrowthRate)
                    .HasColumnName("GrowthRate")
                    .IsRequired();

                parametersBuilder.Property(p => p.TimeHorizonMonths)
                    .HasColumnName("TimeHorizonMonths")
                    .IsRequired();

                parametersBuilder.Property(p => p.CostPerCustomer)
                    .HasColumnName("CostPerCustomer")
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                parametersBuilder.Property(p => p.AdditionalParameters)
                    .HasColumnName("AdditionalParameters")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions)null));
            });

            // Value Object Configuration - ProjectedRevenueData (stored as JSON collection)
            builder.Property(x => x.Projections)
                .HasColumnName("ProjectionsJson")
                .HasConversion(
                    v => JsonSerializer.Serialize(v.Select(p => new
                    {
                        p.MonthlyRecurringRevenue,
                        p.AnnualRecurringRevenue,
                        p.TotalRevenue,
                        p.NetRevenue,
                        p.CustomerCount,
                        p.CustomerAcquisitionCost,
                        p.LifetimeValue,
                        p.ChurnRate,
                        p.ProjectionDate
                    }), (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<dynamic>>(v, (JsonSerializerOptions)null)
                        .Select(p => new ProjectedRevenueData(
                            p.GetProperty("MonthlyRecurringRevenue").GetDecimal(),
                            p.GetProperty("AnnualRecurringRevenue").GetDecimal(),
                            p.GetProperty("TotalRevenue").GetDecimal(),
                            p.GetProperty("NetRevenue").GetDecimal(),
                            p.GetProperty("CustomerCount").GetInt32(),
                            p.GetProperty("CustomerAcquisitionCost").GetDecimal(),
                            p.GetProperty("LifetimeValue").GetDecimal(),
                            p.GetProperty("ChurnRate").GetDouble(),
                            p.GetProperty("ProjectionDate").GetDateTime()))
                        .ToList())
                .HasMaxLength(8000);

            // Indexes
            builder.HasIndex(x => x.RevenueModelId);
            builder.HasIndex(x => new { x.RevenueModelId, x.Name }).IsUnique();
        }
    }
}
