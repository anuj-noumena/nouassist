using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement.Configurations
{
    public class ProjectDependencyConfiguration : IEntityTypeConfiguration<ProjectDependency>
    {
        public void Configure(EntityTypeBuilder<ProjectDependency> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "ProjectDependencies",
                WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // === RELATIONSHIPS ===
            builder.Property(x => x.ProjectId)
                .IsRequired();

            builder.Property(x => x.DependsOnProjectId)
                .IsRequired();

            builder.Property(x => x.TenantId)
                .IsRequired(false);

            // === DEPENDENCY INFO ===
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

            builder.Property(x => x.Strength)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Notes)
                .HasMaxLength(5000);

            builder.Property(x => x.ExpectedResolutionDate)
                .IsRequired(false);

            // === RESOLUTION ===
            builder.Property(x => x.ResolvedAt)
                .IsRequired(false);

            builder.Property(x => x.ResolvedBy)
                .IsRequired(false);

            builder.Property(x => x.ResolutionNote)
                .HasMaxLength(2000);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(ProjectDependency.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);

            // === INDEXES ===
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.DependsOnProjectId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => new { x.ProjectId, x.Status });
            builder.HasIndex(x => new { x.IsBlocking, x.Status });
        }
    }
}