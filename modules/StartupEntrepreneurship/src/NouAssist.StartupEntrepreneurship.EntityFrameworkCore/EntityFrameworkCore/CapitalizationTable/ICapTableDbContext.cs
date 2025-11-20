using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for CapTable module
    /// </summary>
    [ConnectionStringName(CapTableDbProperties.ConnectionStringName)]
    public interface ICapTableDbContext : IEfCoreDbContext
    {
        // DbSet for each Aggregate Root in this module
        DbSet<Aggregates.CapTable> CapTables { get; }
        DbSet<SecurityHolder> SecurityHolders { get; }
        DbSet<SecurityIssuance> SecurityIssuances { get; }
        DbSet<FundingRound> FundingRounds { get; }
        DbSet<OptionPool> OptionPools { get; }
    }
}
