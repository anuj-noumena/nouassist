using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.PersonalDevelopment.Habits.EntityFrameworkCore
{
    public static class HabitsDbContextModelCreatingExtensions
    {
        public static void ConfigureHabits(
            this ModelBuilder builder,
            HabitsDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(HabitsDbContext).Assembly);
        }
    }
}
