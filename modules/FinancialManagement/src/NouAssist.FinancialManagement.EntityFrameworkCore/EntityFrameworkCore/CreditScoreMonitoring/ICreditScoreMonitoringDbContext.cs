using Microsoft.EntityFrameworkCore;
using NouAssist.CreditScoreMonitoring.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CreditScoreMonitoring.EntityFrameworkCore
{
    [ConnectionStringName(CreditScoreMonitoringDbProperties.ConnectionStringName)]
    public interface ICreditScoreMonitoringDbContext : IEfCoreDbContext
    {
        DbSet<CreditReport> CreditReports { get; }
        DbSet<CreditDispute> CreditDisputes { get; }
    }
}
