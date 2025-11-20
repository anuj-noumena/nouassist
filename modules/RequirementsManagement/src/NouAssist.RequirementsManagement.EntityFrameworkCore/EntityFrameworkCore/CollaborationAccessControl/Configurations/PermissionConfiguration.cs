using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CollaborationDbProperties.DbTablePrefix + "Permissions", 
                            CollaborationDbProperties.DbSchema);

            // Primary Key is a computed key consisting of multiple properties
            builder.HasKey("Type", "Resource_Type", "Resource_Id");

            // Properties
            builder.Property(x => x.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            // Value Object Mapping - TargetResource
            builder.OwnsOne(x => x.Resource, resourceBuilder =>
            {
                resourceBuilder.Property(r => r.Type)
                    .HasColumnName("Resource_Type")
                    .HasConversion<string>()
                    .IsRequired()
                    .HasMaxLength(50);

                resourceBuilder.Property(r => r.Id)
                    .HasColumnName("Resource_Id");
            });

            // Indexes
            builder.HasIndex("Type");
            builder.HasIndex("Resource_Type", "Resource_Id");
        }
    }
}
