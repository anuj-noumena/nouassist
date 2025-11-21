using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces
{
    /// <summary>
    /// Service for designing and configuring experiments
    /// </summary>
    public interface IExperimentDesignService
    {
        /// <summary>
        /// Creates a new experiment with initial configuration
        /// </summary>
        Task<GrowthExperiment> CreateExperimentAsync(
            string name,
            string description,
            ExperimentGoal goal,
            TargetMetric primaryMetric,
            IEnumerable<TargetMetric> secondaryMetrics = null);
        
        /// <summary>
        /// Adds a hypothesis to an experiment
        /// </summary>
        Task AddHypothesisAsync(
            Guid experimentId,
            string statement,
            string rationale);
        
        /// <summary>
        /// Adds a control variant to an experiment
        /// </summary>
        Task<ExperimentVariant> AddControlVariantAsync(
            Guid experimentId,
            string name,
            string description,
            double trafficAllocation,
            string configuration,
            string url = null);
        
        /// <summary>
        /// Adds a treatment variant to an experiment
        /// </summary>
        Task<ExperimentVariant> AddTreatmentVariantAsync(
            Guid experimentId,
            string name,
            string description,
            double trafficAllocation,
            string configuration,
            string url = null);
        
        /// <summary>
        /// Sets the experiment as ready to run
        /// </summary>
        Task SetExperimentReadyAsync(Guid experimentId);
    }
}
