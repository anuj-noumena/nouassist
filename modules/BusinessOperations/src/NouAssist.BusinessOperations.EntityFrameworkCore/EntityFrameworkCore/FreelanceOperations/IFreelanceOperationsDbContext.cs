using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore
{
    [ConnectionStringName(FreelanceOperationsDbProperties.ConnectionStringName)]
    public interface IFreelanceOperationsDbContext : IEfCoreDbContext
    {
        // DbSet for each Aggregate Root
        DbSet<FreelanceProject> Projects { get; }
        DbSet<FreelanceClient> Clients { get; }
        DbSet<Proposal> Proposals { get; }
        DbSet<UpworkEngagement> UpworkEngagements { get; }
    }
}
