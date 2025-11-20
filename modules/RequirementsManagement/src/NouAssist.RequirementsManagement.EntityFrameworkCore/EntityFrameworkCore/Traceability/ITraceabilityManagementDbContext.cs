using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Traceability.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for the TraceabilityManagement module
    /// </summary>
    [ConnectionStringName(TraceabilityManagementDbProperties.ConnectionStringName)]
    public interface ITraceabilityManagementDbContext : IEfCoreDbContext
    {
        /// <summary>
        /// DbSet for TraceLink aggregate
        /// </summary>
        DbSet<TraceLink> TraceLinks { get; }
    }
}
