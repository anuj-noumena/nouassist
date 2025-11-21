using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Constants;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Constants;
using NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Extensions;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Portfolio aggregate
    /// </summary>
    public class PortfolioAggregateConfiguration : IEntityTypeConfiguration<PortfolioAggregate>
    {
        public void Configure(EntityTypeBuilder<PortfolioAggregate> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "Portfolios",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ContractPortfolioManagementConsts.Portfolio.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ContractPortfolioManagementConsts.Portfolio.MaxDescriptionLength);

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Handle collection of value objects
            builder.Ignore(x => x.Goals);
            builder.Ignore(x => x.Metrics);
            builder.Ignore(x => x.SharingLinks);
            
            // Configure JSON serialization for collections
            builder.ConfigurePortfolioGoals();
            builder.ConfigurePerformanceMetrics();
            builder.ConfigureSharingLinks();

            // Relationships
            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(i => i.PortfolioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Categories)
                .WithOne()
                .HasForeignKey(c => c.PortfolioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Risks)
                .WithOne()
                .HasForeignKey(r => r.PortfolioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ComplianceRecords)
                .WithOne()
                .HasForeignKey(cr => cr.PortfolioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasIndex(x => x.IsActive);
        }
    }
}
