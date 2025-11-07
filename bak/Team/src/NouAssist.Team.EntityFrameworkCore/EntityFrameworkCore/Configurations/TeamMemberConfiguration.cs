using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Team.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Team.EntityFrameworkCore.Configurations
{
    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                TeamDbProperties.DbTablePrefix + "TeamMembers",
                TeamDbProperties.DbSchema
            );

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TeamId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.Role)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<byte>();

            builder.Property(x => x.Title)
                .HasMaxLength(200);

            builder.Property(x => x.LeftReason)
                .HasMaxLength(1000);

            builder.Property(x => x.Permissions)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

            // Indexes
            builder.HasIndex(x => x.TeamId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.TeamId, x.UserId }).IsUnique();
            builder.HasIndex(x => x.Status);
        }
    }
}
