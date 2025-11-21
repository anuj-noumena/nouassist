using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Configurations
{
    public class ProposalVersionConfiguration : IEntityTypeConfiguration<ProposalVersion>
    {
        public void Configure(EntityTypeBuilder<ProposalVersion> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(FreelanceOperationsDbProperties.DbTablePrefix + "ProposalVersions", 
                            FreelanceOperationsDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.ProposalId).IsRequired();
            builder.Property(x => x.VersionNumber).IsRequired().HasMaxLength(20);
            builder.Property(x => x.VersionName)
                .IsRequired()
                .HasMaxLength(FreelanceOperationsConsts.Proposal.MaxVersionNameLength);
            
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.Notes);

            // Value Object Mappings
            builder.OwnsOne(x => x.ContentSummary, summaryBuilder => 
            {
                summaryBuilder.Property(s => s.Summary).HasColumnName("Summary")
                    .IsRequired()
                    .HasMaxLength(FreelanceOperationsConsts.Proposal.MaxSummaryLength);
                
                // Handle collection of strings within ValueObject
                summaryBuilder.Property<string>("_keySections")
                    .HasColumnName("KeySections")
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
            });

            builder.OwnsOne(x => x.Pricing, pricingBuilder => 
            {
                pricingBuilder.Property(p => p.BillingType).HasColumnName("BillingType")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);
                    
                pricingBuilder.OwnsOne(p => p.HourlyRate, hourlyRateBuilder => 
                {
                    hourlyRateBuilder.Property(r => r.Amount).HasColumnName("HourlyRate");
                    hourlyRateBuilder.Property(r => r.Currency).HasColumnName("HourlyRateCurrency").HasMaxLength(10);
                });
                
                pricingBuilder.Property(p => p.FixedPrice).HasColumnName("FixedPrice");
                pricingBuilder.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(10);
                pricingBuilder.Property(p => p.EstimatedHours).HasColumnName("EstimatedHours");
                pricingBuilder.Property(p => p.PaymentTerms).HasColumnName("PaymentTerms");
            });

            builder.OwnsOne(x => x.DocumentId, docBuilder => 
            {
                docBuilder.Property(d => d.Value).HasColumnName("DocumentId").IsRequired();
            });

            // Indexing
            builder.HasIndex(x => x.ProposalId);
            builder.HasIndex(x => x.VersionNumber);
            builder.HasIndex(x => x.CreatedAt);
        }
    }
}
