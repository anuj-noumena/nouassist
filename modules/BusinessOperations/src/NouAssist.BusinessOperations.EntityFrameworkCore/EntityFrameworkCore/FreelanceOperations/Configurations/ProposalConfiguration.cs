using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Configurations
{
    public class ProposalConfiguration : IEntityTypeConfiguration<Proposal>
    {
        public void Configure(EntityTypeBuilder<Proposal> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(FreelanceOperationsDbProperties.DbTablePrefix + "Proposals", 
                            FreelanceOperationsDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(FreelanceOperationsConsts.Proposal.MaxTitleLength);

            builder.Property(x => x.ClientId).IsRequired();
            builder.Property(x => x.ProjectId);
            
            // Enum Mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Value Object Mapping
            builder.OwnsOne(x => x.TemplateReference, templateBuilder => 
            {
                templateBuilder.Property(t => t.TemplateId).HasColumnName("TemplateId").IsRequired();
                templateBuilder.Property(t => t.TemplateName).HasColumnName("TemplateName").IsRequired().HasMaxLength(100);
                templateBuilder.Property(t => t.Version).HasColumnName("TemplateVersion").IsRequired().HasMaxLength(20);
            });

            // Regular Properties
            builder.Property(x => x.SentDate);
            builder.Property(x => x.ViewedDate);
            builder.Property(x => x.ResponseDate);
            builder.Property(x => x.ExpirationDate);
            builder.Property(x => x.CurrentVersionId);

            // Relationship Mapping for versions
            builder.HasMany(x => x.Versions)
                   .WithOne()
                   .HasForeignKey(v => v.ProposalId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.ExpirationDate);
        }
    }
}
