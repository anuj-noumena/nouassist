using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.ContractPortfolioManagement.Constants;
using NouAssist.ContractPortfolioManagement.Entities.Portfolio;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for PortfolioItem entity
    /// </summary>
    public class PortfolioItemConfiguration : IEntityTypeConfiguration<PortfolioItem>
    {
        public void Configure(EntityTypeBuilder<PortfolioItem> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "PortfolioItems",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.PortfolioId).IsRequired();
            builder.Property(x => x.CategoryId);
            builder.Property(x => x.Progress).HasPrecision(5, 2);
            builder.Property(x => x.Notes).HasMaxLength(1000);
            builder.Property(x => x.AddedDate).IsRequired();
            builder.Property(x => x.LastUpdatedDate);

            // Value Object Mappings
            builder.OwnsOne(x => x.Reference, refBuilder =>
            {
                refBuilder.Property(r => r.ItemId)
                    .HasColumnName("ItemId")
                    .IsRequired();

                refBuilder.Property(r => r.ItemType)
                    .HasColumnName("ItemType")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                refBuilder.Property(r => r.ItemName)
                    .HasColumnName("ItemName")
                    .IsRequired()
                    .HasMaxLength(256);
            });

            // Indexing
            builder.HasIndex(x => x.PortfolioId);
            builder.HasIndex(x => x.CategoryId);
            builder.HasIndex(x => new { x.PortfolioId, x.Reference.ItemId, x.Reference.ItemType }).IsUnique();
        }
    }

    /// <summary>
    /// Entity configuration for PortfolioCategory entity
    /// </summary>
    public class PortfolioCategoryConfiguration : IEntityTypeConfiguration<PortfolioCategory>
    {
        public void Configure(EntityTypeBuilder<PortfolioCategory> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "PortfolioCategories",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.PortfolioId).IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ContractPortfolioManagementConsts.PortfolioCategory.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ContractPortfolioManagementConsts.PortfolioCategory.MaxDescriptionLength);

            builder.Property(x => x.ColorCode)
                .HasMaxLength(30);

            // Indexing
            builder.HasIndex(x => x.PortfolioId);
            builder.HasIndex(x => new { x.PortfolioId, x.Name }).IsUnique();
        }
    }

    /// <summary>
    /// Entity configuration for PortfolioRisk entity
    /// </summary>
    public class PortfolioRiskConfiguration : IEntityTypeConfiguration<PortfolioRisk>
    {
        public void Configure(EntityTypeBuilder<PortfolioRisk> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "PortfolioRisks",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.PortfolioId).IsRequired();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

            // Value Object Mappings
            builder.OwnsOne(x => x.Assessment, assessmentBuilder =>
            {
                assessmentBuilder.Property(a => a.Level)
                    .HasColumnName("RiskLevel")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                assessmentBuilder.Property(a => a.Description)
                    .HasColumnName("Description")
                    .IsRequired()
                    .HasMaxLength(1000);

                assessmentBuilder.Property(a => a.MitigationPlan)
                    .HasColumnName("MitigationPlan")
                    .HasMaxLength(2000);

                assessmentBuilder.Property(a => a.Probability)
                    .HasColumnName("Probability")
                    .HasPrecision(4, 3)
                    .IsRequired();

                assessmentBuilder.Property(a => a.Impact)
                    .HasColumnName("Impact")
                    .HasPrecision(5, 2)
                    .IsRequired();

                assessmentBuilder.Property(a => a.AssessmentDate)
                    .HasColumnName("AssessmentDate")
                    .IsRequired();
            });

            // Indexing
            builder.HasIndex(x => x.PortfolioId);
            builder.HasIndex(x => new { x.PortfolioId, x.Assessment.Level });
        }
    }

    /// <summary>
    /// Entity configuration for PortfolioComplianceRecord entity
    /// </summary>
    public class PortfolioComplianceRecordConfiguration : IEntityTypeConfiguration<PortfolioComplianceRecord>
    {
        public void Configure(EntityTypeBuilder<PortfolioComplianceRecord> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "PortfolioComplianceRecords",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.PortfolioId).IsRequired();
            builder.Property(x => x.IsCompliant).IsRequired();
            builder.Property(x => x.EvidenceDescription).HasMaxLength(2000);
            builder.Property(x => x.AssessmentDate).IsRequired();

            // Value Object Mappings
            builder.OwnsOne(x => x.Rule, ruleBuilder =>
            {
                ruleBuilder.Property(r => r.Code)
                    .HasColumnName("RuleCode")
                    .IsRequired()
                    .HasMaxLength(50);

                ruleBuilder.Property(r => r.Name)
                    .HasColumnName("RuleName")
                    .IsRequired()
                    .HasMaxLength(200);

                ruleBuilder.Property(r => r.Description)
                    .HasColumnName("RuleDescription")
                    .HasMaxLength(1000);

                ruleBuilder.Property(r => r.RequirementText)
                    .HasColumnName("RequirementText")
                    .IsRequired()
                    .HasMaxLength(2000);

                ruleBuilder.Property(r => r.IsMandatory)
                    .HasColumnName("IsMandatory")
                    .IsRequired();
            });

            // Indexing
            builder.HasIndex(x => x.PortfolioId);
            builder.HasIndex(x => x.IsCompliant);
            builder.HasIndex(x => new { x.PortfolioId, x.Rule.Code }).IsUnique();
        }
    }
}
