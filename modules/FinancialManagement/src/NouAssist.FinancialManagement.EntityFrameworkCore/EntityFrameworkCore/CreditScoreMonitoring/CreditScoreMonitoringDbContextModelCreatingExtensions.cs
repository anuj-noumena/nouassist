using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.CreditScoreMonitoring.EntityFrameworkCore
{
    public static class CreditScoreMonitoringDbContextModelCreatingExtensions
    {
        public static void ConfigureCreditScoreMonitoring(
            this ModelBuilder builder,
            CreditScoreMonitoringDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(CreditScoreMonitoringDbContext).Assembly);
        }
    }
}
