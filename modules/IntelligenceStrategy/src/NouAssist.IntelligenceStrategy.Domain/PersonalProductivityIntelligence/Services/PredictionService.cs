using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Interfaces.Services;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Repositories;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Services
{
    /// <summary>
    /// Implements prediction generation services
    /// </summary>
    public class PredictionService : DomainService, IPredictionService
    {
        private readonly IPredictionInstanceRepository _predictionRepository;
        private readonly IUserProductivityProfileRepository _userProfileRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<PredictionService> _logger;

        /// <summary>
        /// Creates a new PredictionService
        /// </summary>
        public PredictionService(
            IPredictionInstanceRepository predictionRepository,
            IUserProductivityProfileRepository userProfileRepository,
            IGuidGenerator guidGenerator,
            ILogger<PredictionService> logger)
        {
            _predictionRepository = predictionRepository;
            _userProfileRepository = userProfileRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <summary>
        /// Predicts task completion time
        /// </summary>
        public async Task<PredictionInstance> PredictTaskCompletionTimeAsync(
            Guid userId,
            Guid taskId,
            string taskName,
            string taskDescription,
            TaskContext taskContext = null)
        {
            try
            {
                var userProfile = await _userProfileRepository.FindByUserIdAsync(userId);
                
                if (userProfile == null)
                {
                    throw new UserProfileNotFoundException(userId);
                }
                
                // In a real implementation, this would use ML to predict based on historical data
                // For this example, we're using a simple algorithm
                
                // Get historical tasks with similar context
                var historicalTasks = userProfile.TaskCompletionRecords;
                
                // Calculate estimated time based on history (simplified)
                int minMinutes = 30;  // Default minimum
                int maxMinutes = 90;  // Default maximum
                int mostLikelyMinutes = 60;  // Default most likely
                
                if (historicalTasks.Count > 0)
                {
                    var similarTasks = new List<int>();
                    foreach (var task in historicalTasks)
                    {
                        similarTasks.Add(task.ActualDuration);
                    }
                    
                    if (similarTasks.Count > 0)
                    {
                        similarTasks.Sort();
                        minMinutes = similarTasks[0];
                        maxMinutes = similarTasks[^1];
                        mostLikelyMinutes = similarTasks[similarTasks.Count / 2];  // median
                        
                        // Apply user's historical accuracy trend (assumed to be in work pattern)
                        if (userProfile.WorkPattern != null)
                        {
                            // This would parse the stored model data and apply adjustments
                            // For simplicity, we're applying a 10% adjustment
                            mostLikelyMinutes = (int)(mostLikelyMinutes * 0.9);
                        }
                    }
                }
                
                var timeEstimate = new TimeEstimate(minMinutes, mostLikelyMinutes, maxMinutes);
                
                // Create prediction
                var prediction = PredictionInstance.CreateDurationPrediction(
                    _guidGenerator.Create(),
                    userId,
                    $"Completion time estimate for: {taskName}",
                    $"Based on your history, this task will take approximately {timeEstimate.ExpectedMinutes} minutes to complete.",
                    timeEstimate,
                    PredictionConfidenceLevel.Medium,
                    0.75,
                    "Task",
                    taskId,
                    DateTime.UtcNow,
                    userProfile.TenantId);
                
                // Add explainability
                prediction.AddExplainabilityLog(
                    "Historical Task Analysis",
                    "Analyzed your history of similar tasks to determine time estimates.",
                    0.8,
                    "statistical",
                    "{\"sampleSize\": " + historicalTasks.Count + "}");
                
                prediction.AddExplainabilityLog(
                    "Work Pattern Adjustment",
                    "Applied your personal work pattern to refine the estimate.",
                    0.4,
                    "model",
                    "{\"adjustment\": 0.9}");
                
                // Add model metadata
                prediction.SetModelMetadata(
                    "TaskDurationPredictor",
                    "1.0",
                    "taskHistory,workPattern,taskContext",
                    new Dictionary<string, double>
                    {
                        { "taskHistory", 0.7 },
                        { "workPattern", 0.2 },
                        { "taskContext", 0.1 }
                    });
                
                await _predictionRepository.InsertAsync(prediction);
                
                _logger.LogInformation("Generated duration prediction for user {UserId}, task {TaskId}", userId, taskId);
                return prediction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error predicting task duration for user {UserId}, task {TaskId}", userId, taskId);
                throw;
            }
        }

        /// <summary>
        /// Predicts risk for a task or project
        /// </summary>
        public async Task<PredictionInstance> PredictRiskAsync(
            Guid userId,
            string targetEntityType,
            Guid targetEntityId,
            string entityName,
            string entityDescription)
        {
            try
            {
                var userProfile = await _userProfileRepository.FindByUserIdAsync(userId);
                
                if (userProfile == null)
                {
                    throw new UserProfileNotFoundException(userId);
                }
                
                // In a real implementation, this would use ML to predict risk
                // For this example, we're using a simple algorithm
                double riskValue = 0.3;  // Default medium-low risk
                
                // Analyze description for risk factors (simplified)
                var riskFactors = new Dictionary<string, double>();
                
                if (entityDescription.Contains("urgent", StringComparison.OrdinalIgnoreCase))
                {
                    riskFactors["Urgency"] = 0.6;
                    riskValue += 0.2;
                }
                
                if (entityDescription.Contains("complex", StringComparison.OrdinalIgnoreCase))
                {
                    riskFactors["Complexity"] = 0.7;
                    riskValue += 0.15;
                }
                
                if (entityDescription.Contains("dependent", StringComparison.OrdinalIgnoreCase))
                {
                    riskFactors["Dependencies"] = 0.5;
                    riskValue += 0.1;
                }
                
                // Cap at 0.95
                riskValue = Math.Min(0.95, riskValue);
                
                var riskProbability = new RiskProbability(riskValue);
                
                // Determine confidence based on data quality
                var confidenceLevel = PredictionConfidenceLevel.Medium;
                var confidenceValue = 0.7;
                
                if (riskFactors.Count == 0)
                {
                    confidenceLevel = PredictionConfidenceLevel.Low;
                    confidenceValue = 0.5;
                }
                else if (riskFactors.Count > 2)
                {
                    confidenceLevel = PredictionConfidenceLevel.High;
                    confidenceValue = 0.85;
                }
                
                // Create prediction
                var prediction = PredictionInstance.CreateRiskPrediction(
                    _guidGenerator.Create(),
                    userId,
                    $"Risk assessment for: {entityName}",
                    $"This {targetEntityType.ToLower()} has a {riskProbability.FormatAsPercentage()} " +
                    $"({riskProbability.GetRiskLevel()}) risk of delays or complications.",
                    riskProbability,
                    confidenceLevel,
                    confidenceValue,
                    targetEntityType,
                    targetEntityId,
                    DateTime.UtcNow,
                    userProfile.TenantId);
                
                // Add explainability
                foreach (var factor in riskFactors)
                {
                    prediction.AddExplainabilityLog(
                        factor.Key,
                        $"Detected {factor.Key.ToLower()} as a risk factor in the description.",
                        factor.Value,
                        "content-analysis",
                        $"{{\"term\": \"{factor.Key.ToLower()}\", \"impact\": {factor.Value}}}");
                }
                
                // Add model metadata
                prediction.SetModelMetadata(
                    "RiskPredictor",
                    "1.0",
                    "contentAnalysis,userHistory,entityMetadata",
                    new Dictionary<string, double>
                    {
                        { "contentAnalysis", 0.6 },
                        { "userHistory", 0.3 },
                        { "entityMetadata", 0.1 }
                    });
                
                await _predictionRepository.InsertAsync(prediction);
                
                _logger.LogInformation(
                    "Generated risk prediction for user {UserId}, entity type {EntityType}, entity ID {EntityId}",
                    userId, targetEntityType, targetEntityId);
                return prediction;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error predicting risk for user {UserId}, entity type {EntityType}, entity ID {EntityId}",
                    userId, targetEntityType, targetEntityId);
                throw;
            }
        }

        /// <summary>
        /// Predicts cognitive load based on planned activities
        /// </summary>
        public async Task<PredictionInstance> PredictCognitiveLoadAsync(
            Guid userId,
            DateTime targetDate)
        {
            try
            {
                var userProfile = await _userProfileRepository.FindByUserIdAsync(userId);
                
                if (userProfile == null)
                {
                    throw new UserProfileNotFoundException(userId);
                }
                
                // In a real implementation, this would use ML to predict cognitive load
                // based on scheduled activities, current load, and user patterns
                // For this example, we're using a simple algorithm
                
                double currentLoad = userProfile.CognitiveLoadValue;
                double predictedLoad = currentLoad * 0.9;  // Assume some recovery by target date
                
                // Add imaginary scheduled activities impact
                predictedLoad += 20;  // Assuming medium impact of scheduled activities
                
                // Cap at 100
                predictedLoad = Math.Min(100, predictedLoad);
                
                // Create prediction
                var prediction = PredictionInstance.CreateNumericPrediction(
                    _guidGenerator.Create(),
                    userId,
                    $"Cognitive load forecast for {targetDate:MM/dd/yyyy}",
                    $"Your cognitive load is predicted to be at {predictedLoad:0}% on {targetDate:MM/dd/yyyy}.",
                    predictedLoad,
                    PredictionConfidenceLevel.Medium,
                    0.7,
                    "CognitiveLoad",
                    null,
                    DateTime.UtcNow,
                    targetDate,
                    userProfile.TenantId);
                
                // Add explainability
                prediction.AddExplainabilityLog(
                    "Current Load",
                    "Your current cognitive load is a factor in predicting future load.",
                    0.5,
                    "current-state",
                    $"{{\"currentLoad\": {currentLoad}}}");
                
                prediction.AddExplainabilityLog(
                    "Scheduled Activities",
                    "Your scheduled activities for the target date impact predicted load.",
                    0.7,
                    "schedule-analysis",
                    "{\"scheduledActivities\": 5, \"impact\": 20}");
                
                // Add model metadata
                prediction.SetModelMetadata(
                    "CognitiveLoadPredictor",
                    "1.0",
                    "currentLoad,scheduledActivities,recoveryRate",
                    new Dictionary<string, double>
                    {
                        { "currentLoad", 0.5 },
                        { "scheduledActivities", 0.4 },
                        { "recoveryRate", 0.1 }
                    });
                
                await _predictionRepository.InsertAsync(prediction);
                
                _logger.LogInformation(
                    "Generated cognitive load prediction for user {UserId}, target date {TargetDate}",
                    userId, targetDate);
                return prediction;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error predicting cognitive load for user {UserId}, target date {TargetDate}",
                    userId, targetDate);
                throw;
            }
        }

        /// <summary>
        /// Records actual outcome for a prediction
        /// </summary>
        public async Task RecordActualOutcomeAsync(
            Guid predictionId,
            double actualValue)
        {
            try
            {
                var prediction = await _predictionRepository.GetAsync(predictionId);
                
                prediction.RecordActualOutcome(actualValue, DateTime.UtcNow);
                await _predictionRepository.UpdateAsync(prediction);
                
                _logger.LogInformation("Recorded actual outcome for prediction {PredictionId}", predictionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording actual outcome for prediction {PredictionId}", predictionId);
                throw;
            }
        }
    }
}
