using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IdentityAccess.Team.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IdentityAccess.EntityFrameworkCore.Team.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<IdentityAccess.Team.Aggregates.Team>
    {
        public void Configure(EntityTypeBuilder<IdentityAccess.Team.Aggregates.Team> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                IdentityAccessDbProperties.DbTablePrefix + "Teams",
                IdentityAccessDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Avatar)
                .HasMaxLength(500);

            builder.Property(x => x.Color)
                .HasMaxLength(50);

            builder.Property(x => x.TeamType)
                .IsRequired()
                .HasConversion<byte>();

            // Admin IDs as JSON
            builder.Property(e => e.AdminIds)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, new System.Text.Json.JsonSerializerOptions()),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, new System.Text.Json.JsonSerializerOptions()))
                .HasColumnName("AdminIds")
                .HasColumnType("jsonb")
                .Metadata.SetValueComparer(new ValueComparer<List<Guid>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            builder.Property(x => x.Metadata)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            // Relationships - Members
            builder.HasMany(typeof(TeamMember).FullName)
                .WithOne()
                .HasForeignKey("TeamId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.TeamType);
            builder.HasIndex(x => x.ParentTeamId);
            builder.HasIndex(x => x.LeaderId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
