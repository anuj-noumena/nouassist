using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Interfaces.Services;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Permissions;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Specifications;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions
{
    /// <summary>
    /// Implementation of the prediction application service
    /// </summary>
    [Authorize]
    public class PredictionAppService : ApplicationService, IPredictionAppService
    {
        private readonly IPredictionInstanceRepository _predictionRepository;
        private readonly IPredictionService _predictionService;
        private readonly ILogger<PredictionAppService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public PredictionAppService(
            IPredictionInstanceRepository predictionRepository,
            IPredictionService predictionService,
            ILogger<PredictionAppService> logger)
        {
            _predictionRepository = predictionRepository;
            _predictionService = predictionService;
            _logger = logger;
            
            LocalizationResource = typeof(PersonalProductivityIntelligenceResource);
        }

        /// <summary>
        /// Gets a prediction by ID
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.Prediction.View)]
        public async Task<PredictionDto> GetAsync(Guid id)
        {
            try
            {
                var prediction = await _predictionRepository.GetAsync(id);
                
                // Check tenant
                if (prediction.TenantId != CurrentTenant.Id)
                {
                    throw new EntityNotFoundException(typeof(PredictionInstance), id);
                }
                
                return MapPredictionToDto(prediction);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["PredictionNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "GetAsync");
                throw;
            }
        }

        /// <summary>
        /// Gets predictions for a user
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.Prediction.View)]
        public async Task<PagedResultDto<PredictionDto>> GetListAsync(GetUserPredictionsDto input)
        {
            try
            {
                var predictions = await _predictionRepository.GetUserPredictionsAsync(
                    input.UserId,
                    input.PredictionType,
                    input.MaxResultCount);
                
                // Apply additional filters if specified
                if (!string.IsNullOrEmpty(input.MinConfidenceLevel) && 
                    Enum.TryParse<PredictionConfidenceLevel>(input.MinConfidenceLevel, out var minConfidence))
                {
                    predictions = predictions.Where(p => p.ConfidenceLevel >= minConfidence).ToList();
                }
                
                if (!string.IsNullOrEmpty(input.TargetEntityType))
                {
                    predictions = predictions.Where(p => p.TargetEntityType == input.TargetEntityType).ToList();
                    
                    if (input.TargetEntityId.HasValue)
                    {
                        predictions = predictions.Where(p => p.TargetEntityId == input.TargetEntityId).ToList();
                    }
                }
                
                // Apply paging
                var pagedPredictions = predictions
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();
                
                // Map to appropriate DTOs based on prediction type
                var predictionDtos = pagedPredictions.Select(MapPredictionToDto).ToList();
                
                return new PagedResultDto<PredictionDto>
                {
                    TotalCount = predictions.Count,
                    Items = predictionDtos
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "GetListAsync");
                throw;
            }
        }

        /// <summary>
        /// Predicts task duration
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.Prediction.Create)]
        public async Task<TaskDurationPredictionDto> PredictTaskDurationAsync(PredictTaskDurationDto input)
        {
            try
            {
                // Map TaskContextDto to TaskContext value object
                TaskContext taskContext = null;
                if (input.TaskContext != null)
                {
                    taskContext = new TaskContext(
                        input.TaskContext.ProjectId,
                        input.TaskContext.TeamId,
                        input.TaskContext.Tags,
                        input.TaskContext.Location,
                        input.TaskContext.EnergyLevel,
                        input.TaskContext.FocusLevel);
                }
                
                var prediction = await _predictionService.PredictTaskCompletionTimeAsync(
                    input.UserId,
                    input.TaskId,
                    input.TaskName,
                    input.TaskDescription,
                    taskContext);
                
                return MapToTaskDurationPredictionDto(prediction);
            }
            catch (UserProfileNotFoundException)
            {
                throw new UserFriendlyException(L["UserProfileNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "PredictTaskDurationAsync");
                throw;
            }
        }

        /// <summary>
        /// Predicts risk
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.Prediction.Create)]
        public async Task<RiskPredictionDto> PredictRiskAsync(PredictRiskDto input)
        {
            try
            {
                var prediction = await _predictionService.PredictRiskAsync(
                    input.UserId,
                    input.TargetEntityType,
                    input.TargetEntityId,
                    input.EntityName,
                    input.EntityDescription);
                
                return MapToRiskPredictionDto(prediction);
            }
            catch (UserProfileNotFoundException)
            {
                throw new UserFriendlyException(L["UserProfileNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "PredictRiskAsync");
                throw;
            }
        }

        /// <summary>
        /// Predicts cognitive load
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.Prediction.Create)]
        public async Task<NumericPredictionDto> PredictCognitiveLoadAsync(PredictCognitiveLoadDto input)
        {
            try
            {
                var prediction = await _predictionService.PredictCognitiveLoadAsync(
                    input.UserId,
                    input.TargetDate);
                
                return MapToNumericPredictionDto(prediction);
            }
            catch (UserProfileNotFoundException)
            {
                throw new UserFriendlyException(L["UserProfileNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "PredictCognitiveLoadAsync");
                throw;
            }
        }

        /// <summary>
        /// Records actual outcome for a prediction
        /// </summary>
        [Authorize(PersonalProductivityIntelligencePermissions.Prediction.RecordOutcome)]
        public async Task RecordActualOutcomeAsync(RecordActualOutcomeDto input)
        {
            try
            {
                await _predictionService.RecordActualOutcomeAsync(
                    input.PredictionId,
                    input.ActualValue);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["PredictionNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex, "RecordActualOutcomeAsync");
                throw;
            }
        }
        
        /// <summary>
        /// Maps a prediction to the appropriate DTO based on its type
        /// </summary>
        private PredictionDto MapPredictionToDto(PredictionInstance prediction)
        {
            if (prediction.PredictionType == "Duration")
            {
                return MapToTaskDurationPredictionDto(prediction);
            }
            else if (prediction.PredictionType == "Risk")
            {
                return MapToRiskPredictionDto(prediction);
            }
            else
            {
                return MapToNumericPredictionDto(prediction);
            }
        }
        
        /// <summary>
        /// Maps a duration prediction to DTO
        /// </summary>
        private TaskDurationPredictionDto MapToTaskDurationPredictionDto(PredictionInstance prediction)
        {
            var dto = ObjectMapper.Map<PredictionInstance, TaskDurationPredictionDto>(prediction);
            
            if (prediction.TimeEstimate != null)
            {
                dto.MinMinutes = prediction.TimeEstimate.MinMinutes;
                dto.MostLikelyMinutes = prediction.TimeEstimate.MostLikelyMinutes;
                dto.MaxMinutes = prediction.TimeEstimate.MaxMinutes;
                dto.ExpectedMinutes = prediction.TimeEstimate.ExpectedMinutes;
                dto.FormattedEstimate = prediction.TimeEstimate.Format();
            }
            
            return dto;
        }
        
        /// <summary>
        /// Maps a risk prediction to DTO
        /// </summary>
        private RiskPredictionDto MapToRiskPredictionDto(PredictionInstance prediction)
        {
            var dto = ObjectMapper.Map<PredictionInstance, RiskPredictionDto>(prediction);
            
            if (prediction.RiskProbability != null)
            {
                dto.RiskValue = prediction.RiskProbability.Value;
                dto.RiskLevel = prediction.RiskProbability.GetRiskLevel();
                dto.FormattedRiskPercentage = prediction.RiskProbability.FormatAsPercentage();
            }
            
            return dto;
        }
        
        /// <summary>
        /// Maps a numeric prediction to DTO
        /// </summary>
        private NumericPredictionDto MapToNumericPredictionDto(PredictionInstance prediction)
        {
            var dto = ObjectMapper.Map<PredictionInstance, NumericPredictionDto>(prediction);
            
            if (prediction.OutcomeValue.HasValue)
            {
                dto.OutcomeValue = prediction.OutcomeValue.Value;
            }
            
            return dto;
        }
        
        private async Task HandleErrorAsync(Exception exception, string methodName)
        {
            _logger.LogError(exception, "Error in {AppServiceName}.{MethodName}: {ErrorMessage}", 
                GetType().Name, methodName, exception.Message);
            await Task.CompletedTask;
        }
    }
}
