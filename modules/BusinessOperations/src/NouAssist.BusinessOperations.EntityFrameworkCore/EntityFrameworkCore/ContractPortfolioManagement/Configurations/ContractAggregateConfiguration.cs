using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Constants;
using NouAssist.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.ContractPortfolioManagement.Constants;
using NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Extensions;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Contract aggregate
    /// </summary>
    public class ContractAggregateConfiguration : IEntityTypeConfiguration<ContractAggregate>
    {
        public void Configure(EntityTypeBuilder<ContractAggregate> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "Contracts",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ContractPortfolioManagementConsts.Contract.MaxTitleLength);

            builder.Property(x => x.ReferenceNumber)
                .IsRequired()
                .HasMaxLength(ContractPortfolioManagementConsts.Contract.MaxReferenceNumberLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ContractPortfolioManagementConsts.Contract.MaxDescriptionLength);

            // Enum Mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.EffectiveDate)
                .IsRequired();

            builder.Property(x => x.ExpirationDate);

            builder.Property(x => x.TotalValue)
                .HasPrecision(18, 2);

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(10);

            // Value Object Mappings
            builder.OwnsOne(x => x.TemplateReference, templateBuilder =>
            {
                templateBuilder.Property(t => t.TemplateId)
                    .HasColumnName("TemplateId")
                    .IsRequired();

                templateBuilder.Property(t => t.TemplateName)
                    .HasColumnName("TemplateName")
                    .IsRequired()
                    .HasMaxLength(256);

                templateBuilder.Property(t => t.Version)
                    .HasColumnName("TemplateVersion")
                    .IsRequired()
                    .HasMaxLength(50);
            });

            builder.OwnsOne(x => x.MainDocumentId, documentBuilder =>
            {
                documentBuilder.Property(d => d.Id)
                    .HasColumnName("DocumentId")
                    .IsRequired();

                documentBuilder.Property(d => d.RepositoryType)
                    .HasColumnName("DocumentRepositoryType")
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // Relationships
            builder.HasMany(x => x.Versions)
                .WithOne()
                .HasForeignKey(v => v.ContractId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Clauses)
                .WithOne()
                .HasForeignKey(c => c.ContractId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Parties)
                .WithOne()
                .HasForeignKey(p => p.ContractId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Amendments)
                .WithOne()
                .HasForeignKey(a => a.ContractId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Alerts)
                .WithOne()
                .HasForeignKey(a => a.ContractId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.ReferenceNumber).IsUnique();
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.EffectiveDate);
            builder.HasIndex(x => x.ExpirationDate);
        }
    }
}
