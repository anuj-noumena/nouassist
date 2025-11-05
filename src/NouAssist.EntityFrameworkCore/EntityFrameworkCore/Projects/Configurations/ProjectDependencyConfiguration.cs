using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Projects.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.EntityFrameworkCore.Projects.Configurations
{
    public class ProjectDependencyConfiguration : IEntityTypeConfiguration<ProjectDependency>
    {
        public void Configure(EntityTypeBuilder<ProjectDependency> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable("ProjectDependencies", "Projects");

            builder.HasKey(x => x.Id);

            // Relationships
            builder.Property(x => x.ProjectId)
                .IsRequired();

            builder.Property(x => x.DependsOnProjectId)
                .IsRequired();

            builder.Property(x => x.TenantId)
                .IsRequired(false);

            // Dependency Info
            builder.Property(x => x.DependencyType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.LagDays)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.IsBlocking)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Notes)
                .HasMaxLength(5000);

            // Resolution
            builder.Property(x => x.ExpectedResolutionDate)
                .IsRequired(false);

            builder.Property(x => x.ActualResolutionDate)
                .IsRequired(false);

            builder.Property(x => x.ResolvedBy)
                .HasMaxLength(256);

            builder.Property(x => x.ApprovalRequestId)
                .IsRequired(false);

            // // Extra Properties
            // builder.Property(x => x.ExtraProperties)
            //     .HasColumnName(nameof(ProjectDependency.ExtraProperties));

            // // Concurrency Stamp
            // builder.Property(x => x.ConcurrencyStamp)
            //     .IsConcurrencyToken()
            //     .HasMaxLength(40);

            // Indexes
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.DependsOnProjectId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => new { x.ProjectId, x.Status });
            builder.HasIndex(x => new { x.IsBlocking, x.Status });
            builder.HasIndex(x => x.TenantId);
        }
    }
}
