using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore.Habits.Configurations
{
    public class RoutineAggregateConfiguration : IEntityTypeConfiguration<Routine>
    {
        public void Configure(EntityTypeBuilder<Routine> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(PersonalDevelopmentDbProperties.DbTablePrefix + "Routines",
                           PersonalDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.TriggerDescription)
                .HasMaxLength(500);

            builder.Property(x => x.TimeOfDay)
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.OwnerId)
                .IsRequired();

            // Scheduled days as JSON
            builder.Property(x => x.ScheduledDays)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<int>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new List<int>()
                )
                .HasColumnType("jsonb");

            // Relationships
            builder.HasMany(x => x.Steps)
                .WithOne()
                .HasForeignKey("RoutineId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => new { x.OwnerId, x.IsActive });
            builder.HasIndex(x => x.TimeOfDay);
            builder.HasIndex(x => x.TenantId);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
