using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Permissions;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Repositories;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments
{
    [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Default)]
    public class GrowthExperimentAppService : ApplicationService, IGrowthExperimentAppService
    {
        private readonly IGrowthExperimentRepository _experimentRepository;
        private readonly IExperimentDesignService _experimentDesignService;
        private readonly IResultsAnalysisService _resultsAnalysisService;
        private readonly ILearningRepositoryService _learningRepositoryService;
        private readonly IGuidGenerator _guidGenerator;

        public GrowthExperimentAppService(
            IGrowthExperimentRepository experimentRepository,
            IExperimentDesignService experimentDesignService,
            IResultsAnalysisService resultsAnalysisService,
            ILearningRepositoryService learningRepositoryService,
            IGuidGenerator guidGenerator)
        {
            _experimentRepository = experimentRepository;
            _experimentDesignService = experimentDesignService;
            _resultsAnalysisService = resultsAnalysisService;
            _learningRepositoryService = learningRepositoryService;
            _guidGenerator = guidGenerator;
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.View)]
        public async Task<GrowthExperimentDto> GetAsync(Guid id)
        {
            try
            {
                var experiment = await _experimentRepository.GetWithDetailsAsync(id);
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting experiment with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.View)]
        public async Task<PagedResultDto<GrowthExperimentDto>> GetListAsync(GetGrowthExperimentListDto input)
        {
            try
            {
                var queryable = await _experimentRepository.GetQueryableAsync();

                // Apply filtering
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    queryable = queryable.Where(e => e.Name.Contains(input.Filter) || 
                                                    e.Description.Contains(input.Filter));
                }

                if (input.Status.HasValue)
                {
                    queryable = queryable.Where(e => e.Status == input.Status.Value);
                }

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(GrowthExperiment.Name))
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);

                var experiments = await AsyncExecuter.ToListAsync(queryable);
                var experimentDtos = ObjectMapper.Map<List<GrowthExperiment>, List<GrowthExperimentDto>>(experiments);

                return new PagedResultDto<GrowthExperimentDto>(totalCount, experimentDtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting experiment list");
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Create)]
        public async Task<GrowthExperimentDto> CreateAsync(CreateGrowthExperimentDto input)
        {
            try
            {
                // Map DTOs to domain objects
                var goal = ObjectMapper.Map<CreateExperimentGoalDto, ExperimentGoal>(input.Goal);
                var primaryMetric = ObjectMapper.Map<CreateTargetMetricDto, TargetMetric>(input.PrimaryMetric);
                
                // Set as primary
                primaryMetric = new TargetMetric(
                    primaryMetric.Name,
                    primaryMetric.Type,
                    primaryMetric.BaselineValue,
                    primaryMetric.TargetValue,
                    isPrimary: true
                );
                
                // Map secondary metrics if provided
                List<TargetMetric> secondaryMetrics = null;
                if (input.SecondaryMetrics != null && input.SecondaryMetrics.Any())
                {
                    secondaryMetrics = ObjectMapper.Map<List<CreateTargetMetricDto>, List<TargetMetric>>(input.SecondaryMetrics);
                }

                // Create experiment using domain service
                var experiment = await _experimentDesignService.CreateExperimentAsync(
                    input.Name,
                    input.Description,
                    goal,
                    primaryMetric,
                    secondaryMetrics);

                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error creating experiment: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating experiment {Name}", input.Name);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Update)]
        public async Task<GrowthExperimentDto> UpdateAsync(Guid id, UpdateGrowthExperimentDto input)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(id);
                
                // Update basic properties (name, description)
                // Note: In a real app, you'd check if the experiment is in a valid state for updates
                experiment.Name = input.Name;
                experiment.Description = input.Description;
                
                await _experimentRepository.UpdateAsync(experiment);
                
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error updating experiment: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating experiment with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _experimentRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException)
            {
                // If entity not found, consider it as deleted
                return;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting experiment with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Update)]
        public async Task<GrowthExperimentDto> AddHypothesisAsync(Guid id, CreateHypothesisDto input)
        {
            try
            {
                await _experimentDesignService.AddHypothesisAsync(id, input.Statement, input.Rationale);
                
                // Get updated experiment
                var experiment = await _experimentRepository.GetWithDetailsAsync(id);
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error adding hypothesis: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding hypothesis to experiment with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Update)]
        public async Task<GrowthExperimentDto> AddVariantAsync(Guid id, CreateExperimentVariantDto input)
        {
            try
            {
                // Map DTO to domain object
                var variantSetup = ObjectMapper.Map<CreateVariantSetupDto, VariantSetup>(input.Setup);
                
                ExperimentVariant variant;
                if (input.Type == Enums.VariantType.Control)
                {
                    variant = await _experimentDesignService.AddControlVariantAsync(
                        id,
                        input.Name,
                        input.Description,
                        input.Setup.TrafficAllocation,
                        input.Setup.Configuration,
                        input.Setup.Url);
                }
                else
                {
                    variant = await _experimentDesignService.AddTreatmentVariantAsync(
                        id,
                        input.Name,
                        input.Description,
                        input.Setup.TrafficAllocation,
                        input.Setup.Configuration,
                        input.Setup.Url);
                }
                
                // Get updated experiment
                var experiment = await _experimentRepository.GetWithDetailsAsync(id);
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error adding variant: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding variant to experiment with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Update)]
        public async Task<GrowthExperimentDto> SetExperimentReadyAsync(Guid id)
        {
            try
            {
                await _experimentDesignService.SetExperimentReadyAsync(id);
                
                // Get updated experiment
                var experiment = await _experimentRepository.GetWithDetailsAsync(id);
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error setting experiment ready: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error setting experiment ready with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Update)]
        public async Task<GrowthExperimentDto> StartExperimentAsync(Guid id)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(id);
                
                if (experiment.Status != Enums.ExperimentStatus.Ready)
                {
                    throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:InvalidExperimentState"]);
                }
                
                experiment.Start(Clock.Now);
                await _experimentRepository.UpdateAsync(experiment);
                
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error starting experiment: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error starting experiment with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Update)]
        public async Task<GrowthExperimentDto> PauseExperimentAsync(Guid id)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(id);
                
                if (experiment.Status != Enums.ExperimentStatus.Running)
                {
                    throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:InvalidExperimentState"]);
                }
                
                experiment.Pause(Clock.Now);
                await _experimentRepository.UpdateAsync(experiment);
                
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error pausing experiment: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error pausing experiment with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Update)]
        public async Task<GrowthExperimentDto> ResumeExperimentAsync(Guid id)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(id);
                
                if (experiment.Status != Enums.ExperimentStatus.Paused)
                {
                    throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:InvalidExperimentState"]);
                }
                
                experiment.Resume(Clock.Now);
                await _experimentRepository.UpdateAsync(experiment);
                
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error resuming experiment: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error resuming experiment with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Update)]
        public async Task<GrowthExperimentDto> CompleteExperimentAsync(Guid id)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(id);
                
                if (experiment.Status != Enums.ExperimentStatus.Running && experiment.Status != Enums.ExperimentStatus.Paused)
                {
                    throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:InvalidExperimentState"]);
                }
                
                experiment.Complete(Clock.Now);
                await _experimentRepository.UpdateAsync(experiment);
                
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error completing experiment: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error completing experiment with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.Update)]
        public async Task<GrowthExperimentDto> TerminateExperimentAsync(Guid id)
        {
            try
            {
                var experiment = await _experimentRepository.GetAsync(id);
                
                if (experiment.Status == Enums.ExperimentStatus.Completed || experiment.Status == Enums.ExperimentStatus.Terminated)
                {
                    throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:InvalidExperimentState"]);
                }
                
                experiment.Terminate(Clock.Now);
                await _experimentRepository.UpdateAsync(experiment);
                
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error terminating experiment: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error terminating experiment with ID {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.ManageResults)]
        public async Task<ExperimentResultDataDto> RecordResultAsync(Guid experimentId, RecordExperimentResultDto input)
        {
            try
            {
                // Verify experiment exists and variant belongs to it
                var experiment = await _experimentRepository.GetWithDetailsAsync(experimentId);
                var variant = experiment.Variants.FirstOrDefault(v => v.Id == input.VariantId);
                
                if (variant == null)
                {
                    throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:VariantNotFound"]);
                }
                
                // Record the result using domain service
                await _resultsAnalysisService.RecordResultDataAsync(
                    input.VariantId,
                    input.MetricName,
                    input.MetricType,
                    input.Value,
                    input.SampleSize,
                    input.StandardDeviation,
                    input.MarginOfError,
                    input.IsPrimary);
                
                // Get the recorded result
                var results = await _experimentRepository.GetResultsForVariantAsync(input.VariantId);
                var result = results.FirstOrDefault(r => r.MetricName == input.MetricName);
                
                if (result == null)
                {
                    throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ResultNotFound"]);
                }
                
                return ObjectMapper.Map<ExperimentResultData, ExperimentResultDataDto>(result);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error recording result: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error recording result for variant {VariantId}", input.VariantId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.View)]
        public async Task<List<ExperimentResultDataDto>> GetResultsForVariantAsync(Guid variantId)
        {
            try
            {
                var results = await _experimentRepository.GetResultsForVariantAsync(variantId);
                return ObjectMapper.Map<List<ExperimentResultData>, List<ExperimentResultDataDto>>(results);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting results for variant {VariantId}", variantId);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.AnalyzeResults)]
        public async Task<StatisticalSignificanceDto> AnalyzeSignificanceAsync(Guid id, string primaryMetricName)
        {
            try
            {
                var significance = await _resultsAnalysisService.AnalyzeSignificanceAsync(id, primaryMetricName);
                return ObjectMapper.Map<StatisticalSignificance, StatisticalSignificanceDto>(significance);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error analyzing significance: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error analyzing significance for experiment {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }

        [Authorize(GrowthExperimentationPermissions.GrowthExperiment.AnalyzeResults)]
        public async Task<GrowthExperimentDto> FinalizeLearningsAsync(Guid id, CreateExperimentLearningsDto input)
        {
            try
            {
                // Create learnings with current user as documented by
                var currentUserId = CurrentUser.Id ?? GuidGenerator.Create();
                
                // Record learnings
                await _learningRepositoryService.RecordLearningsAsync(
                    id,
                    input.Insights,
                    input.UnexpectedOutcomes,
                    input.Recommendations,
                    currentUserId);
                
                // Get updated experiment
                var experiment = await _experimentRepository.GetWithDetailsAsync(id);
                return ObjectMapper.Map<GrowthExperiment, GrowthExperimentDto>(experiment);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:ExperimentNotFound"], 
                    GrowthExperimentationDomainErrorCodes.ExperimentNotFound);
            }
            catch (BusinessException ex)
            {
                Logger.LogWarning(ex, "Business error finalizing learnings: {ErrorCode}, {ErrorMessage}", ex.Code, ex.Message);
                throw new UserFriendlyException(L[ex.Code ?? "GrowthExperimentation:ErrorMessage:DefaultError"], ex.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error finalizing learnings for experiment {Id}", id);
                throw new UserFriendlyException(L["GrowthExperimentation:ErrorMessage:DefaultError"]);
            }
        }
    }
}
