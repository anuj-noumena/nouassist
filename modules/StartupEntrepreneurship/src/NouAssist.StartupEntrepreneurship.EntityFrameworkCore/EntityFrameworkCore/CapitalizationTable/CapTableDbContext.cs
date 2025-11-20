using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore
{
    /// <summary>
    /// DbContext implementation for CapTable module
    /// </summary>
    [ConnectionStringName(CapTableDbProperties.ConnectionStringName)]
    public class CapTableDbContext : AbpDbContext<CapTableDbContext>, ICapTableDbContext
    {
        // Implement DbSet properties from the interface
        public DbSet<Aggregates.CapTable> CapTables { get; set; }
        public DbSet<SecurityHolder> SecurityHolders { get; set; }
        public DbSet<SecurityIssuance> SecurityIssuances { get; set; }
        public DbSet<FundingRound> FundingRounds { get; set; }
        public DbSet<OptionPool> OptionPools { get; set; }

        public CapTableDbContext(DbContextOptions<CapTableDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly using the extension method
            builder.ConfigureCapTable(this);
        }
    }
}
