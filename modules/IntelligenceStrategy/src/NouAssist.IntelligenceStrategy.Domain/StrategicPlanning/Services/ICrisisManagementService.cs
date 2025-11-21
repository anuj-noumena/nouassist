using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Services
{
    /// <summary>
    /// Domain service interface for crisis management
    /// </summary>
    public interface ICrisisManagementService
    {
        Task<CrisisManagementPlan> CreatePlanAsync(
            string name,
            string description,
            CrisisLevel maxCrisisLevel,
            DateTime nextReviewDate);

        Task ActivateCrisisPlanAsync(Guid planId, Guid activatedBy, string reason = null);
        
        Task<List<CrisisManagementPlan>> GetPlansForCrisisLevelAsync(CrisisLevel crisisLevel);
        
        Task<bool> HasActiveCrisisPlanAsync();
    }
}
