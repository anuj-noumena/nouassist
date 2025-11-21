using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces
{
    /// <summary>
    /// Service for managing experiment learnings
    /// </summary>
    public interface ILearningRepositoryService
    {
        /// <summary>
        /// Records learnings from an experiment
        /// </summary>
        Task RecordLearningsAsync(
            Guid experimentId,
            string insights,
            string unexpectedOutcomes,
            string recommendations,
            Guid documentedBy);
            
        /// <summary>
        /// Gets all learnings for experiments
        /// </summary>
        Task<List<ExperimentLearnings>> GetAllLearningsAsync();
            
        /// <summary>
        /// Gets learnings for a specific experiment
        /// </summary>
        Task<ExperimentLearnings> GetLearningsForExperimentAsync(Guid experimentId);
    }
}
