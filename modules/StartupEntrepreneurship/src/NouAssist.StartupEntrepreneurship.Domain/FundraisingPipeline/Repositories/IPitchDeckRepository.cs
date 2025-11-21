using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories
{
    /// <summary>
    /// Repository interface for PitchDeck aggregate
    /// </summary>
    public interface IPitchDeckRepository : IRepository<PitchDeck, Guid>
    {
        Task<PitchDeck> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default);
        
        Task<List<PitchDeck>> GetByFundraisingRoundAsync(
            Guid fundraisingRoundId, 
            CancellationToken cancellationToken = default);
        
        Task<PitchDeck> GetLatestForFundraisingRoundAsync(
            Guid fundraisingRoundId, 
            CancellationToken cancellationToken = default);
        
        Task<List<PitchPresentation>> GetPresentationsForProspectAsync(
            Guid prospectId, 
            CancellationToken cancellationToken = default);
    }
}
