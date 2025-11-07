using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CollaborationHub.TeamCollaboration.EntityFrameworkCore.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team.Aggregates.Team>
    {
        public void Configure(EntityTypeBuilder<Team.Aggregates.Team> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(
                TeamDbProperties.DbTablePrefix + "Teams",
                TeamDbProperties.DbSchema
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
            builder.Property("_adminIds")
                .HasColumnName("AdminIds")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions)null)
                );

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
