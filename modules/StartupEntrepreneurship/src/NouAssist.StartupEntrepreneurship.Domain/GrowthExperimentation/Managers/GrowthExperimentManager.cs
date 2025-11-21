using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Exceptions;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Managers
{
    /// <summary>
    /// Manager for GrowthExperiment aggregate - handles complex business logic
    /// </summary>
    public class GrowthExperimentManager : DomainService
    {
        private readonly IGrowthExperimentRepository _experimentRepository;
        private readonly ILogger<GrowthExperimentManager> _logger;

        public GrowthExperimentManager(
            IGrowthExperimentRepository experimentRepository,
            ILogger<GrowthExperimentManager> logger)
        {
            _experimentRepository = experimentRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new growth experiment with validation
        /// </summary>
        public async Task<GrowthExperiment> CreateAsync(
            string title,
            string hypothesis,
            string description,
            DateTime startDate,
            DateTime endDate,
            Guid ownerId)
        {
            // Validate dates
            if (endDate <= startDate)
            {
                throw new GrowthExperimentationDomainException(
                    "GrowthExperimentation:InvalidDateRange",
                    "End date must be after start date.");
            }

            // Check for duplicate title
            var existing = await _experimentRepository.FindByTitleAsync(
                title,
                CurrentTenant.Id);

            if (existing != null)
            {
                throw new GrowthExperimentationDomainException(
                    "GrowthExperimentation:DuplicateTitle",
                    $"An experiment with title '{title}' already exists.");
            }

            var experiment = GrowthExperiment.Create(
                GuidGenerator.Create(),
                title,
                hypothesis,
                description,
                startDate,
                endDate,
                ownerId,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new growth experiment {ExperimentId} with title '{Title}'",
                experiment.Id, experiment.Title);

            return experiment;
        }

        /// <summary>
        /// Updates experiment details
        /// </summary>
        public async Task<GrowthExperiment> UpdateAsync(
            Guid experimentId,
            string? newTitle = null,
            string? newDescription = null,
            string? newHypothesis = null)
        {
            var experiment = await _experimentRepository.GetAsync(experimentId);

            // Business rule: Cannot update running or completed experiments
            if (experiment.Status == ExperimentStatus.Running)
            {
                throw new ExperimentInvalidStateException(
                    "Cannot update a running experiment.");
            }

            if (experiment.Status == ExperimentStatus.Completed)
            {
                throw new ExperimentInvalidStateException(
                    "Cannot update a completed experiment.");
            }

            experiment.UpdateBasicInfo(
                newTitle ?? experiment.Title,
                newDescription ?? experiment.Description,
                newHypothesis ?? experiment.Hypothesis);

            await _experimentRepository.UpdateAsync(experiment);

            _logger.LogInformation(
                "Updated growth experiment {ExperimentId}",
                experimentId);

            return experiment;
        }

        /// <summary>
        /// Starts the experiment
        /// </summary>
        public async Task<GrowthExperiment> StartAsync(Guid experimentId)
        {
            var experiment = await _experimentRepository.GetAsync(experimentId);

            // Business rule: Must have at least 2 variants
            if (experiment.Variants.Count < 2)
            {
                throw new InsufficientVariantsException(
                    "Experiment must have at least 2 variants to start.");
            }

            experiment.Start(Clock.Now);
            await _experimentRepository.UpdateAsync(experiment);

            _logger.LogInformation(
                "Started growth experiment {ExperimentId}",
                experimentId);

            return experiment;
        }

        /// <summary>
        /// Completes the experiment
        /// </summary>
        public async Task<GrowthExperiment> CompleteAsync(
            Guid experimentId,
            string conclusion,
            Guid? winningVariantId = null)
        {
            var experiment = await _experimentRepository.GetAsync(experimentId);

            // Business rule: Can only complete running experiments
            if (experiment.Status != ExperimentStatus.Running)
            {
                throw new ExperimentInvalidStateException(
                    $"Cannot complete experiment in {experiment.Status} state.");
            }

            experiment.Complete(Clock.Now, conclusion, winningVariantId);
            await _experimentRepository.UpdateAsync(experiment);

            _logger.LogInformation(
                "Completed growth experiment {ExperimentId} with conclusion: {Conclusion}",
                experimentId, conclusion);

            return experiment;
        }

        /// <summary>
        /// Pauses the experiment
        /// </summary>
        public async Task<GrowthExperiment> PauseAsync(Guid experimentId, string reason)
        {
            var experiment = await _experimentRepository.GetAsync(experimentId);

            // Business rule: Can only pause running experiments
            if (experiment.Status != ExperimentStatus.Running)
            {
                throw new ExperimentInvalidStateException(
                    "Can only pause running experiments.");
            }

            experiment.Pause(reason);
            await _experimentRepository.UpdateAsync(experiment);

            _logger.LogInformation(
                "Paused growth experiment {ExperimentId}. Reason: {Reason}",
                experimentId, reason);

            return experiment;
        }

        /// <summary>
        /// Deletes an experiment (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid experimentId, bool hardDelete = false)
        {
            var experiment = await _experimentRepository.GetAsync(experimentId);

            // Business rule: Cannot delete running experiments
            if (experiment.Status == ExperimentStatus.Running && !hardDelete)
            {
                throw new GrowthExperimentationDomainException(
                    "GrowthExperimentation:CannotDeleteRunningExperiment",
                    "Cannot delete a running experiment. Pause it first or use hard delete.");
            }

            if (hardDelete)
            {
                await _experimentRepository.DeleteAsync(experiment);
                _logger.LogWarning("Hard deleted growth experiment {ExperimentId}", experimentId);
            }
            else
            {
                await _experimentRepository.DeleteAsync(experiment, autoSave: true);
                _logger.LogInformation("Soft deleted growth experiment {ExperimentId}", experimentId);
            }
        }
    }
}
