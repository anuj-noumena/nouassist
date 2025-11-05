using Microsoft.EntityFrameworkCore;
using NouAssist.GTD.Aggregates;
using NouAssist.GTD.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.GTD.EntityFrameworkCore
{
    [ConnectionStringName(GtdDbProperties.ConnectionStringName)]
    public class GtdDbContext : AbpDbContext<GtdDbContext>, IGtdDbContext
    {
        public DbSet<GtdHorizon> GtdHorizons { get; set; }
        public DbSet<GtdHorizonAlignment> GtdHorizonAlignments { get; set; }
        public DbSet<SomedayMaybeItem> SomedayMaybeItems { get; set; }
        public DbSet<SomedayMaybeReview> SomedayMaybeReviews { get; set; }
        public DbSet<TicklerItem> TicklerItems { get; set; }
        public DbSet<TicklerProcessingLog> TicklerProcessingLogs { get; set; }
        public DbSet<WaitingFor> WaitingFors { get; set; }
        public DbSet<Followup> Followups { get; set; }

        public GtdDbContext(DbContextOptions<GtdDbContext> options)
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
