using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore
{
    [ConnectionStringName(FundraisingPipelineDbProperties.ConnectionStringName)]
    public class FundraisingPipelineDbContext : 
        AbpDbContext<FundraisingPipelineDbContext>, IFundraisingPipelineDbContext
    {
        // Implement DbSet properties from the interface
        public DbSet<Prospect> Prospects { get; set; }
        public DbSet<FundraisingRound> FundraisingRounds { get; set; }
        public DbSet<PitchDeck> PitchDecks { get; set; }
        public DbSet<TermSheetOffer> TermSheetOffers { get; set; }

        public FundraisingPipelineDbContext(DbContextOptions<FundraisingPipelineDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly
            builder.ConfigureFundraisingPipeline();
        }
    }
}
