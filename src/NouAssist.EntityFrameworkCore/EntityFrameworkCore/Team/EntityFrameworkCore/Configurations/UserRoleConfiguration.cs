using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Team.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Team.EntityFrameworkCore.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                TeamDbProperties.DbTablePrefix + "UserRoles",
                TeamDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.RoleId).IsRequired();

            builder.Property(x => x.Scope)
                .HasMaxLength(100);

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.RoleId);
            builder.HasIndex(x => new { x.UserId, x.RoleId });
            builder.HasIndex(x => new { x.Scope, x.ScopeId });
            builder.HasIndex(x => x.ExpiresAt);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
