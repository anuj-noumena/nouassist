using Microsoft.EntityFrameworkCore;
using NouAssist.AuditTrail.Aggregates;
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
