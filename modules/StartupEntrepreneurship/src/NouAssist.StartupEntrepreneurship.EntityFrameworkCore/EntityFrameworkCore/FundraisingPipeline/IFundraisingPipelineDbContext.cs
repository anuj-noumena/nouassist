using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore
{
    [ConnectionStringName(FundraisingPipelineDbProperties.ConnectionStringName)]
    public interface IFundraisingPipelineDbContext : IEfCoreDbContext
    {
        DbSet<Prospect> Prospects { get; }
        DbSet<FundraisingRound> FundraisingRounds { get; }
        DbSet<PitchDeck> PitchDecks { get; }
        DbSet<TermSheetOffer> TermSheetOffers { get; }
    }
}
