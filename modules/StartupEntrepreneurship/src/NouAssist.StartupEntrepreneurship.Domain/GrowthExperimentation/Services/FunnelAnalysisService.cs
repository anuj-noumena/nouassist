using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Exceptions;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services
{
    /// <summary>
    /// Implementation of the funnel analysis service
    /// </summary>
    public class FunnelAnalysisService : DomainService, IFunnelAnalysisService
    {
        private readonly IConversionFunnelDefinitionRepository _funnelRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<FunnelAnalysisService> _logger;

        public FunnelAnalysisService(
            IConversionFunnelDefinitionRepository funnelRepository,
            IGuidGenerator guidGenerator,
            ILogger<FunnelAnalysisService> logger)
        {
            _funnelRepository = funnelRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ConversionFunnelDefinition> CreateFunnelAsync(
            string name,
            string description)
        {
            try
            {
                var funnel = ConversionFunnelDefinition.Create(
                    _guidGenerator.Create(),
                    name,
                    description,
                    CurrentTenant.Id);

                await _funnelRepository.InsertAsync(funnel);

                _logger.LogInformation(
                    "Created new conversion funnel {FunnelId} with name {FunnelName}",
                    funnel.Id,
                    funnel.Name);

                return funnel;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create conversion funnel with name {FunnelName}",
                    name);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<FunnelStage> AddFunnelStageAsync(
            Guid funnelId,
            string name,
            string description,
            int order,
            string criteria,
            bool isExitStage = false)
        {
            try
            {
                var funnel = await _funnelRepository.GetAsync(funnelId);

                if (funnel == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:FunnelNotFound")
                        .WithData("funnelId", funnelId);
                }

                var stageDefinition = new StageDefinition(name, description, order, criteria);
                var stage = funnel.AddStage(_guidGenerator.Create(), stageDefinition, isExitStage);

                await _funnelRepository.UpdateAsync(funnel);

                _logger.LogInformation(
                    "Added stage {StageName} (order {Order}) to funnel {FunnelId}",
                    name,
                    order,
                    funnelId);

                return stage;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:FunnelNotFound")
                    .WithData("funnelId", funnelId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to add stage to funnel {FunnelId}",
                    funnelId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateStageConversionRateAsync(
            Guid funnelId,
            Guid stageId,
            int enteredCount,
            int completedCount)
        {
            try
            {
                var funnel = await _funnelRepository.GetWithStagesAsync(funnelId);

                if (funnel == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:FunnelNotFound")
                        .WithData("funnelId", funnelId);
                }

                var stage = funnel.Stages.FirstOrDefault(s => s.Id == stageId);
                if (stage == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:StageNotFound")
                        .WithData("stageId", stageId)
                        .WithData("funnelId", funnelId);
                }

                var conversionRate = new ConversionRatePerStage(stageId, enteredCount, completedCount, Clock.Now);
                funnel.UpdateStageConversionRate(stageId, conversionRate);

                await _funnelRepository.UpdateAsync(funnel);

                _logger.LogInformation(
                    "Updated conversion rate for stage {StageId} in funnel {FunnelId} to {Rate}",
                    stageId,
                    funnelId,
                    conversionRate.ConversionRate);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:FunnelNotFound")
                    .WithData("funnelId", funnelId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to update conversion rate for stage {StageId} in funnel {FunnelId}",
                    stageId,
                    funnelId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, object>> GetFunnelPerformanceAsync(Guid funnelId)
        {
            try
            {
                var funnel = await _funnelRepository.GetWithStagesAsync(funnelId);

                if (funnel == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:FunnelNotFound")
                        .WithData("funnelId", funnelId);
                }

                // Calculate performance metrics
                var performance = new Dictionary<string, object>
                {
                    ["funnelId"] = funnel.Id,
                    ["funnelName"] = funnel.Name,
                    ["overallConversionRate"] = funnel.OverallConversionRate,
                    ["stageMetrics"] = funnel.Stages
                        .OrderBy(s => s.Definition.Order)
                        .Select(s => new
                        {
                            stageId = s.Id,
                            stageName = s.Definition.Name,
                            order = s.Definition.Order,
                            enteredCount = s.ConversionRate?.EnteredCount ?? 0,
                            completedCount = s.ConversionRate?.CompletedCount ?? 0,
                            conversionRate = s.ConversionRate?.ConversionRate ?? 0,
                            isEntryStage = s.IsEntryStage,
                            isExitStage = s.IsExitStage
                        })
                        .ToList()
                };

                _logger.LogInformation(
                    "Retrieved performance metrics for funnel {FunnelId}",
                    funnelId);

                return performance;
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:FunnelNotFound")
                    .WithData("funnelId", funnelId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to get performance metrics for funnel {FunnelId}",
                    funnelId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ValidateFunnelAsync(Guid funnelId)
        {
            try
            {
                var funnel = await _funnelRepository.GetWithStagesAsync(funnelId);

                if (funnel == null)
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:FunnelNotFound")
                        .WithData("funnelId", funnelId);
                }

                funnel.Validate();

                _logger.LogInformation(
                    "Validated funnel {FunnelId} successfully",
                    funnelId);
            }
            catch (EntityNotFoundException)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:FunnelNotFound")
                    .WithData("funnelId", funnelId);
            }
            catch (InvalidFunnelConfigurationException ex)
            {
                _logger.LogWarning(
                    "Funnel {FunnelId} validation failed: {Message}",
                    funnelId,
                    ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to validate funnel {FunnelId}",
                    funnelId);
                throw;
            }
        }
    }
}
