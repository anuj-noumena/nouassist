using System;
using System.Threading.Tasks;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services.Interfaces
{
    /// <summary>
    /// Interface for service that handles prospect scoring
    /// </summary>
    public interface IProspectScoringService
    {
        Task<int> CalculateEngagementScoreAsync(Guid prospectId);
        Task<int> CalculateFitScoreAsync(Guid prospectId);
        Task<int> CalculateTimelinessScoreAsync(Guid prospectId);
        Task UpdateAllScoresAsync(Guid prospectId);
    }
}
