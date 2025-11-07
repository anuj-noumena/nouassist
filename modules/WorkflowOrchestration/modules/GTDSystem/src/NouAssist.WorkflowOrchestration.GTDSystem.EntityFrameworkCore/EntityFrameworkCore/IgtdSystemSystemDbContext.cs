using Microsoft.EntityFrameworkCore;
using NouAssist.WorkflowOrchestration.GTDSystem.Aggregates;
using NouAssist.WorkflowOrchestration.GTDSystem.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkflowOrchestration.GTDSystem.EntityFrameworkCore
{
    [ConnectionStringName(GtdDbProperties.ConnectionStringName)]
    public class IgtdSystemSystemDbContext : AbpDbContext<IgtdSystemSystemDbContext>, IGTDSystemDbContext
    {
        public DbSet<GtdHorizon> GtdHorizons { get; set; }
        public DbSet<GtdHorizonAlignment> GtdHorizonAlignments { get; set; }
        public DbSet<SomedayMaybeItem> SomedayMaybeItems { get; set; }
        public DbSet<SomedayMaybeReview> SomedayMaybeReviews { get; set; }
        public DbSet<TicklerItem> TicklerItems { get; set; }
        public DbSet<TicklerProcessingLog> TicklerProcessingLogs { get; set; }
        public DbSet<WaitingFor> WaitingFors { get; set; }
        public DbSet<Followup> Followups { get; set; }

        public IgtdSystemSystemDbContext(DbContextOptions<IgtdSystemSystemDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureGtd(this);
        }
    }
}
