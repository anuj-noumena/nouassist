using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Constants;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract;
using NouAssist.ContractPortfolioManagement.Constants;
using NouAssist.ContractPortfolioManagement.Entities.Contract;
using NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Extensions;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for ContractVersion entity
    /// </summary>
    public class ContractVersionConfiguration : IEntityTypeConfiguration<ContractVersion>
    {
        public void Configure(EntityTypeBuilder<ContractVersion> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "ContractVersions",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ContractId).IsRequired();
            builder.Property(x => x.CreationTime).IsRequired();

            // Value Object Mappings
            builder.OwnsOne(x => x.Metadata, metadataBuilder =>
            {
                metadataBuilder.Property(m => m.VersionNumber)
                    .HasColumnName("VersionNumber")
                    .IsRequired();

                metadataBuilder.Property(m => m.VersionLabel)
                    .HasColumnName("VersionLabel")
                    .IsRequired()
                    .HasMaxLength(100);

                metadataBuilder.Property(m => m.ChangeReason)
                    .HasColumnName("ChangeReason")
                    .HasMaxLength(500);
            });

            builder.OwnsOne(x => x.DocumentId, documentBuilder =>
            {
                documentBuilder.Property(d => d.Id)
                    .HasColumnName("DocumentId")
                    .IsRequired();

                documentBuilder.Property(d => d.RepositoryType)
                    .HasColumnName("DocumentRepositoryType")
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // Indexing
            builder.HasIndex(x => x.ContractId);
            builder.HasIndex(x => x.CreationTime);
        }
    }

    /// <summary>
    /// Entity configuration for ContractClause entity
    /// </summary>
    public class ContractClauseConfiguration : IEntityTypeConfiguration<ContractClause>
    {
        public void Configure(EntityTypeBuilder<ContractClause> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "ContractClauses",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ContractId).IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ContractPortfolioManagementConsts.ContractClause.MaxTitleLength);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(ContractPortfolioManagementConsts.ContractClause.MaxContentLength);

            builder.Property(x => x.Reference)
                .HasMaxLength(100);

            builder.Property(x => x.IsMandatory)
                .IsRequired();

            // Indexing
            builder.HasIndex(x => x.ContractId);
            builder.HasIndex(x => x.Reference);
        }
    }

    /// <summary>
    /// Entity configuration for ContractParty entity
    /// </summary>
    public class ContractPartyConfiguration : IEntityTypeConfiguration<ContractParty>
    {
        public void Configure(EntityTypeBuilder<ContractParty> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "ContractParties",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ContractId).IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ContractPortfolioManagementConsts.ContractParty.MaxNameLength);

            builder.Property(x => x.Identifier)
                .IsRequired()
                .HasMaxLength(ContractPortfolioManagementConsts.ContractParty.MaxIdentifierLength);

            builder.Property(x => x.ContactInfo)
                .HasMaxLength(ContractPortfolioManagementConsts.ContractParty.MaxContactInfoLength);

            builder.Property(x => x.PartyType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Indexing
            builder.HasIndex(x => x.ContractId);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Identifier);
        }
    }

    /// <summary>
    /// Entity configuration for ContractAmendment entity
    /// </summary>
    public class ContractAmendmentConfiguration : IEntityTypeConfiguration<ContractAmendment>
    {
        public void Configure(EntityTypeBuilder<ContractAmendment> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "ContractAmendments",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ContractId).IsRequired();
            builder.Property(x => x.CreationTime).IsRequired();

            // Value Object Mappings
            builder.OwnsOne(x => x.Details, detailsBuilder =>
            {
                detailsBuilder.Property(d => d.Title)
                    .HasColumnName("Title")
                    .IsRequired()
                    .HasMaxLength(200);

                detailsBuilder.Property(d => d.Description)
                    .HasColumnName("Description")
                    .IsRequired()
                    .HasMaxLength(2000);

                detailsBuilder.Property(d => d.EffectiveDate)
                    .HasColumnName("EffectiveDate")
                    .IsRequired();

                detailsBuilder.ConfigureAffectedSections();
            });

            builder.OwnsOne(x => x.DocumentId, documentBuilder =>
            {
                documentBuilder.Property(d => d.Id)
                    .HasColumnName("DocumentId")
                    .IsRequired();

                documentBuilder.Property(d => d.RepositoryType)
                    .HasColumnName("DocumentRepositoryType")
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // Indexing
            builder.HasIndex(x => x.ContractId);
            builder.HasIndex(x => x.CreationTime);
        }
    }

    /// <summary>
    /// Entity configuration for ContractAlert entity
    /// </summary>
    public class ContractAlertConfiguration : IEntityTypeConfiguration<ContractAlert>
    {
        public void Configure(EntityTypeBuilder<ContractAlert> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                ContractPortfolioManagementDbProperties.DbTablePrefix + "ContractAlerts",
                ContractPortfolioManagementDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ContractId).IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.AlertType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.IsTriggered)
                .IsRequired();

            builder.Property(x => x.LastTriggeredTime);

            // Value Object Mappings
            builder.OwnsOne(x => x.Condition, conditionBuilder =>
            {
                conditionBuilder.Property(c => c.TriggerDate)
                    .HasColumnName("TriggerDate")
                    .IsRequired();

                conditionBuilder.Property(c => c.DaysInAdvance)
                    .HasColumnName("DaysInAdvance")
                    .IsRequired();

                conditionBuilder.Property(c => c.IsRecurring)
                    .HasColumnName("IsRecurring")
                    .IsRequired();

                conditionBuilder.Property(c => c.Frequency)
                    .HasColumnName("Frequency")
                    .HasMaxLength(50);
            });

            // Indexing
            builder.HasIndex(x => x.ContractId);
            builder.HasIndex(x => x.AlertType);
            builder.HasIndex(x => x.IsTriggered);
        }
    }
}
