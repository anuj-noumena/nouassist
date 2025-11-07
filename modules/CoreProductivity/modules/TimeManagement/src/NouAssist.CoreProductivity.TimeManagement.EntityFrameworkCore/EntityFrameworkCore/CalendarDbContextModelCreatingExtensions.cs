using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore
{
    public static class CalendarDbContextModelCreatingExtensions
    {
        public static void ConfigureCalendar(
            this ModelBuilder builder,
            CalendarDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(CalendarDbContext).Assembly);

            // Any additional module-wide configurations can go here
        }
    }
}
