using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits;
using NouAssist.PersonalDevelopment.Habits.Entities;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore.Habits.Configurations
{
    public class HabitResponseConfiguration : IEntityTypeConfiguration<HabitResponse>
    {
        public void Configure(EntityTypeBuilder<HabitResponse> builder)
        {
            builder.ToTable(PersonalDevelopmentDbProperties.DbTablePrefix + "HabitResponses",
                           PersonalDevelopmentDbProperties.DbSchema);

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
