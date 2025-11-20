using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Traceability.EntityFrameworkCore
{
    /// <summary>
    /// EF Core DbContext for the TraceabilityManagement module
    /// </summary>
    [ConnectionStringName(TraceabilityManagementDbProperties.ConnectionStringName)]
    public class TraceabilityManagementDbContext 
        : AbpDbContext<TraceabilityManagementDbContext>, ITraceabilityManagementDbContext
    {
        /// <summary>
        /// DbSet for TraceLink aggregate
        /// </summary>
        public DbSet<TraceLink> TraceLinks { get; set; }

        public TraceabilityManagementDbContext(DbContextOptions<TraceabilityManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base to apply ABP conventions
            base.OnModelCreating(builder);

            // Configure the TraceabilityManagement module
            builder.ConfigureTraceabilityManagement();
        }
    }
}
