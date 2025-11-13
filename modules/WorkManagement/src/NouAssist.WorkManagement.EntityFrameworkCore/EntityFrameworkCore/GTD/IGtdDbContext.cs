using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.GTD
{
    [ConnectionStringName(GtdDbProperties.ConnectionStringName)]
    public interface IGtdDbContext : IEfCoreDbContext
    {
        DbSet<GtdHorizon> GtdHorizons { get; }
        DbSet<GtdHorizonAlignment> GtdHorizonAlignments { get; }
        DbSet<SomedayMaybeItem> SomedayMaybeItems { get; }
        DbSet<SomedayMaybeReview> SomedayMaybeReviews { get; }
        DbSet<TicklerItem> TicklerItems { get; }
        DbSet<TicklerProcessingLog> TicklerProcessingLogs { get; }
        DbSet<WaitingFor> WaitingFors { get; }
        DbSet<Followup> Followups { get; }
    }
}
