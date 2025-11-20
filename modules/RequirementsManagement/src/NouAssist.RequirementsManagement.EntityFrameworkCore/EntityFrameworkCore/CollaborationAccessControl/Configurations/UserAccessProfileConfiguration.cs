using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Entities;
using NouAssist.RequirementsManagement.CollaborationAccessControl.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Configurations
{
    public class UserAccessProfileConfiguration : IEntityTypeConfiguration<UserAccessProfile>
    {
        public void Configure(EntityTypeBuilder<UserAccessProfile> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(CollaborationDbProperties.DbTablePrefix + "UserAccessProfiles", 
                            CollaborationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

            // Value Object Mapping
            builder.OwnsOne(x => x.UserId, userIdBuilder =>
            {
                userIdBuilder.Property(u => u.Value)
                    .HasColumnName("UserId")
                    .IsRequired();
            });

            // Relationships
            builder.HasMany(x => x.Roles)
                .WithOne()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Index on UserId for faster lookups
            builder.HasIndex("UserId").IsUnique();
        }
    }
}
