using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Configurations
{
    public class ProjectMilestoneConfiguration : IEntityTypeConfiguration<ProjectMilestone>
    {
        public void Configure(EntityTypeBuilder<ProjectMilestone> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(FreelanceOperationsDbProperties.DbTablePrefix + "Milestones", 
                            FreelanceOperationsDbProperties.DbSchema);

            // Primary Key 
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.ProjectId).IsRequired();
            builder.Property(x => x.Deadline);
            builder.Property(x => x.IsCompleted).IsRequired();
            builder.Property(x => x.CompletedDate);
            builder.Property(x => x.IsInvoiced).IsRequired();
            builder.Property(x => x.InvoiceId);

            // Value Object Mapping
            builder.OwnsOne(x => x.Details, detailsBuilder => 
            {
                detailsBuilder.Property(d => d.Title).HasColumnName("Title").IsRequired();
                detailsBuilder.Property(d => d.Description).HasColumnName("Description").IsRequired();
                detailsBuilder.Property(d => d.Price).HasColumnName("Price").IsRequired();
                detailsBuilder.Property(d => d.Currency).HasColumnName("Currency").HasMaxLength(10).IsRequired();
                detailsBuilder.Property(d => d.AcceptanceCriteria).HasColumnName("AcceptanceCriteria");
            });

            // Indexing
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.IsCompleted);
            builder.HasIndex(x => x.Deadline);
        }
    }
}
