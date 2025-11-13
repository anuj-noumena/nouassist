using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.TimeManagement.Meetings.EntityFrameworkCore
{
    public static class MeetingsDbContextModelCreatingExtensions
    {
        public static void ConfigureMeetings(
            this ModelBuilder builder,
            MeetingsDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(MeetingsDbContext).Assembly);
        }
    }
}
