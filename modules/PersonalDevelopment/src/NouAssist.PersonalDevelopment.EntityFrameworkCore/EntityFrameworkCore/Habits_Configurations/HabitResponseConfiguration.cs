using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits.Entities;

namespace NouAssist.PersonalDevelopment.Habits.EntityFrameworkCore.Configurations
{
    public class HabitResponseConfiguration : IEntityTypeConfiguration<HabitResponse>
    {
        public void Configure(EntityTypeBuilder<HabitResponse> builder)
        {
            builder.ToTable(HabitsDbProperties.DbTablePrefix + "HabitResponses",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.HabitId)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Details)
                .HasMaxLength(2000);

            builder.Property(x => x.DurationMinutes);

            builder.HasIndex(x => x.HabitId);
        }
    }
}
