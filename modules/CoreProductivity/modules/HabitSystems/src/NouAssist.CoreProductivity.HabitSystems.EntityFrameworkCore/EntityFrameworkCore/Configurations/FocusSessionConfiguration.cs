using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CoreProductivity.HabitSystems.EntityFrameworkCore.Configurations
{
    public class FocusSessionConfiguration : IEntityTypeConfiguration<FocusSession>
    {
        public void Configure(EntityTypeBuilder<FocusSession> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(HabitsDbProperties.DbTablePrefix + "FocusSessions",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(x => x.CompletedPomodoros)
                .IsRequired();

            builder.Property(x => x.TotalFocusTime)
                .IsRequired();

            builder.Property(x => x.TaskId);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.OwnerId, x.Date })
                .IsUnique();
            builder.HasIndex(x => x.TaskId);
            builder.HasIndex(x => x.TenantId);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
