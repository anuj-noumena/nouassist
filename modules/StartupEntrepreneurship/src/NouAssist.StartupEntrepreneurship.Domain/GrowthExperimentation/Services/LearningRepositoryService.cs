using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Exceptions;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services
{
    /// <summary>
    /// Implementation of the learning repository service
    /// </summary>
    public class LearningRepositoryService : DomainService, ILearningRepositoryService
    {
        private readonly IGrowthExperimentRepository _experimentRepository;
        private readonly ILogger<LearningRepositoryService> _logger;

        public LearningRepositoryService(
            IGrowthExperimentRepository experimentRepository,
            ILogger<LearningRepositoryService> logger)
        {
            _experimentRepository = experimentRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task RecordLearningsAsync(
            Guid experimentId,
            string insights,
            string unexpectedOutcomes,
            string recommendations,
            Guid documentedBy)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(experimentId);

                if (experiment == null)
                {
                    throw new ExperimentNotFoundException(experimentId);
                }

                if (experiment.Status != ExperimentStatus.AnalysisInProgress && 
                    experiment.Status != ExperimentStatus.Completed)
                {
                    throw new ExperimentInvalidStateException(
                        experimentId, 
                        experiment.Status, 
                        ExperimentStatus.AnalysisInProgress, 
                        ExperimentStatus.Completed);
                }

                var learnings = new ExperimentLearnings(
                    insights,
                    unexpectedOutcomes,
                    recommendations,
                    documentedBy,
                    Clock.Now);

                if (experiment.Status == ExperimentStatus.AnalysisInProgress)
                {
                    // Complete the experiment with learnings
                    experiment.FinalizeAnalysis(learnings);
                }
                else
                {
                    // Update existing learnings
                    var updatedExperiment = experiment;
                    updatedExperiment.Learnings = learnings;
                    experiment = updatedExperiment;
                }

                await _experimentRepository.UpdateAsync(experiment);

                _logger.LogInformation(
                    "Recorded learnings for experiment {ExperimentId}",
                    experimentId);
            }
            catch (EntityNotFoundException)
            {
                throw new ExperimentNotFoundException(experimentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to record learnings for experiment {ExperimentId}",
                    experimentId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<ExperimentLearnings>> GetAllLearningsAsync()
        {
            try
            {
                var experiments = await _experimentRepository.GetByStatusAsync(ExperimentStatus.Completed);
                var learnings = experiments
                    .Where(e => e.Learnings != null)
                    .Select(e => e.Learnings)
                    .ToList();

                _logger.LogInformation(
                    "Retrieved {Count} experiment learnings",
                    learnings.Count);

                return learnings;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to retrieve all experiment learnings");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ExperimentLearnings> GetLearningsForExperimentAsync(Guid experimentId)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(experimentId);

                if (experiment == null)
                {
                    throw new ExperimentNotFoundException(experimentId);
                }

                if (experiment.Learnings == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:NoLearningsRecorded")
                        .WithData("experimentId", experimentId);
                }

                _logger.LogInformation(
                    "Retrieved learnings for experiment {ExperimentId}",
                    experimentId);

                return experiment.Learnings;
            }
            catch (EntityNotFoundException)
            {
                throw new ExperimentNotFoundException(experimentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to retrieve learnings for experiment {ExperimentId}",
                    experimentId);
                throw;
            }
        }
    }
}
