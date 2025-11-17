using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.AuditTrail.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.AuditTrail.EntityFrameworkCore
{
    [ConnectionStringName(AuditTrailDbProperties.ConnectionStringName)]
    public interface IAuditTrailDbContext : IEfCoreDbContext
    {
        DbSet<AuditEntry> AuditEntries { get; }
    }
}
