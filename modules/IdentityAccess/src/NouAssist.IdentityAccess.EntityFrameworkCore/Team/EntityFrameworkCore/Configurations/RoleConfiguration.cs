using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IdentityAccess.Team.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IdentityAccess.Team.EntityFrameworkCore.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                TeamDbProperties.DbTablePrefix + "Roles",
                TeamDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            // Permissions as JSON
            builder.Property("_permissions")
                .HasColumnName("Permissions")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.OrganizationId);
            builder.HasIndex(x => x.IsSystem);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
