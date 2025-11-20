using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Constants;
using NouAssist.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Extensions;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for PortfolioRoadmap aggregate
    /// </summary>
    public class PortfolioRoadmapAggregateConfiguration : IEntityTypeConfiguration<PortfolioRoadmapAggregate>
    {
        public void Configure(EntityTypeBuilder<PortfolioRoadmapAggregate> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "PortfolioRoadmaps",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ContractPortfolioManagementConsts.PortfolioRoadmap.MaxTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ContractPortfolioManagementConsts.PortfolioRoadmap.MaxDescriptionLength);

            builder.Property(x => x.PortfolioId)
                .IsRequired();

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.Milestones)
                .WithOne()
                .HasForeignKey(m => m.RoadmapId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Dependencies)
                .WithOne()
                .HasForeignKey(d => d.RoadmapId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.PortfolioId);
            builder.HasIndex(x => x.StartDate);
            builder.HasIndex(x => x.EndDate);
        }
    }

    /// <summary>
    /// Entity configuration for RoadmapMilestone entity
    /// </summary>
    public class RoadmapMilestoneConfiguration : IEntityTypeConfiguration<RoadmapMilestone>
    {
        public void Configure(EntityTypeBuilder<RoadmapMilestone> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "RoadmapMilestones",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.RoadmapId).IsRequired();
            
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(x => x.Description)
                .HasMaxLength(1000);
                
            builder.Property(x => x.PortfolioItemId);

            // Value Object Mappings
            builder.OwnsOne(x => x.Date, dateBuilder =>
            {
                dateBuilder.Property(d => d.PlannedDate)
                    .HasColumnName("PlannedDate")
                    .IsRequired();

                dateBuilder.Property(d => d.ActualDate)
                    .HasColumnName("ActualDate");

                dateBuilder.Property(d => d.IsCompleted)
                    .HasColumnName("IsCompleted")
                    .IsRequired();
            });

            // Handle skill requirements collection
            builder.Ignore(x => x.SkillRequirements);
            
            // Configure JSON serialization for collection
            builder.ConfigureResourceSkillRequirements();

            // Indexing
            builder.HasIndex(x => x.RoadmapId);
            builder.HasIndex(x => new { x.RoadmapId, x.Date.PlannedDate });
            builder.HasIndex(x => x.PortfolioItemId);
        }
    }

    /// <summary>
    /// Entity configuration for CrossItemDependency entity
    /// </summary>
    public class CrossItemDependencyConfiguration : IEntityTypeConfiguration<CrossItemDependency>
    {
        public void Configure(EntityTypeBuilder<CrossItemDependency> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "CrossItemDependencies",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.RoadmapId).IsRequired();
            builder.Property(x => x.SourceMilestoneId).IsRequired();
            builder.Property(x => x.TargetMilestoneId).IsRequired();
            
            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Indexing
            builder.HasIndex(x => x.RoadmapId);
            builder.HasIndex(x => x.SourceMilestoneId);
            builder.HasIndex(x => x.TargetMilestoneId);
            builder.HasIndex(x => new { x.SourceMilestoneId, x.TargetMilestoneId }).IsUnique();
        }
    }
}
