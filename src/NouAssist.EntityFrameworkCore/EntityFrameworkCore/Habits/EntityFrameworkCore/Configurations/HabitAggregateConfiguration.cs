using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Habits.Aggregates;
using NouAssist.Habits.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Habits.EntityFrameworkCore.Configurations
{
    public class HabitAggregateConfiguration : IEntityTypeConfiguration<Habit>
    {
        public void Configure(EntityTypeBuilder<Habit> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(HabitsDbProperties.DbTablePrefix + "Habits", 
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Goal)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.HabitType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.AreaId);
            builder.Property(x => x.ContextId);

            // Collections stored as JSON or separate table
            // Storing scheduled days as JSON for simplicity
            builder.Property(x => x.ScheduledDays)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<DayOfWeek>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new List<DayOfWeek>()
                )
                .HasColumnType("jsonb");

            // Relationships - One-to-Many for child entities
            builder.HasMany(x => x.Cues)
                .WithOne()
                .HasForeignKey("HabitId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Responses)
                .WithOne()
                .HasForeignKey("HabitId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Rewards)
                .WithOne()
                .HasForeignKey("HabitId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Strategies)
                .WithOne()
                .HasForeignKey("HabitId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => new { x.OwnerId, x.IsActive });
            builder.HasIndex(x => x.AreaId);
            builder.HasIndex(x => x.ContextId);

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
