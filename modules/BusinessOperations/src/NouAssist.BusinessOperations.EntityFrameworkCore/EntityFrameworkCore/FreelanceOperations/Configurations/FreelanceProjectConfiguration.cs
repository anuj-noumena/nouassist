using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Configurations
{
    public class FreelanceProjectConfiguration : IEntityTypeConfiguration<FreelanceProject>
    {
        public void Configure(EntityTypeBuilder<FreelanceProject> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(FreelanceOperationsDbProperties.DbTablePrefix + "Projects", 
                            FreelanceOperationsDbProperties.DbSchema);

            // Primary Key (handled by Entity<TKey> / AggregateRoot<TKey>)
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(FreelanceOperationsConsts.Project.MaxNameLength);

            // Enum Mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.BillingType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mappings
            builder.OwnsOne(x => x.HourlyRate, hourlyRateBuilder => 
            {
                hourlyRateBuilder.Property(r => r.Amount).HasColumnName("HourlyRate");
                hourlyRateBuilder.Property(r => r.Currency).HasColumnName("Currency").HasMaxLength(10);
            });

            builder.OwnsOne(x => x.Scope, scopeBuilder => 
            {
                scopeBuilder.Property(s => s.Description).HasColumnName("ScopeDescription")
                    .HasMaxLength(FreelanceOperationsConsts.Project.MaxDescriptionLength);
                scopeBuilder.Property(s => s.IsFlexible).HasColumnName("ScopeIsFlexible");
                
                // Handle collection of strings within ValueObject
                scopeBuilder.Property<string>("_deliverables")
                    .HasColumnName("ScopeDeliverables")
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
            });

            // Regular Properties
            builder.Property(x => x.FixedPrice).HasColumnName("FixedPrice");
            builder.Property(x => x.Currency).HasMaxLength(10);
            builder.Property(x => x.StartDate);
            builder.Property(x => x.EndDate);
            builder.Property(x => x.CompletionDate);

            // Relationship Mapping (One-to-Many for child entities)
            builder.HasMany(x => x.TimeEntries)
                   .WithOne()
                   .HasForeignKey("ProjectId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Milestones)
                   .WithOne()
                   .HasForeignKey(m => m.ProjectId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Foreign Key relationship to Client
            builder.HasOne<FreelanceClient>()
                   .WithMany()
                   .HasForeignKey(p => p.ClientId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            // Indexing
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.Status);
        }
    }
}
