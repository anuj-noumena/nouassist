using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Constants;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CollaborationDbProperties.DbTablePrefix + "Roles", 
                            CollaborationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Description)
                .HasMaxLength(CollaborationConsts.MaxRoleDescriptionLength);

            builder.Property(x => x.IsSystemRole)
                .IsRequired();

            // Value Object Mapping
            builder.OwnsOne(x => x.Name, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(CollaborationConsts.MaxRoleNameLength);
            });

            // Relationships
            builder.HasMany(x => x.Permissions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex("Name").IsUnique();
        }
    }
}
