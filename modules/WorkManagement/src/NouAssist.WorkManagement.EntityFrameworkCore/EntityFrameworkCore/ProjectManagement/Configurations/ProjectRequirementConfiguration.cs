using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement.Configurations
{
    public class ProjectRequirementConfiguration : IEntityTypeConfiguration<ProjectRequirement>
    {
        public void Configure(EntityTypeBuilder<ProjectRequirement> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable("ProjectRequirements", "Projects");

            builder.HasKey(x => x.Id);

            // Relationships
            builder.Property(x => x.ProjectId)
                .IsRequired();

            // Basic Info
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Description)
                .HasMaxLength(5000);

            builder.Property(x => x.RequirementType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // // Extra Properties
            // builder.Property(x => x.ExtraProperties)
            //     .HasColumnName(nameof(ProjectRequirement.ExtraProperties));
            //
            // // Concurrency Stamp
            // builder.Property(x => x.ConcurrencyStamp)
            //     .IsConcurrencyToken()
            //     .HasMaxLength(40);

            // Indexes
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.Status);
        }
    }
}
