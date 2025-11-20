using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Interfaces.Services
{
    /// <summary>
    /// Defines operations for generating predictions
    /// </summary>
    public interface IPredictionService
    {
        /// <summary>
        /// Predicts task completion time
        /// </summary>
        Task<PredictionInstance> PredictTaskCompletionTimeAsync(
            Guid userId,
            Guid taskId,
            string taskName,
            string taskDescription,
            TaskContext taskContext = null);
        
        /// <summary>
        /// Predicts risk for a task or project
        /// </summary>
        Task<PredictionInstance> PredictRiskAsync(
            Guid userId,
            string targetEntityType,
            Guid targetEntityId,
            string entityName,
            string entityDescription);
        
        /// <summary>
        /// Predicts cognitive load based on planned activities
        /// </summary>
        Task<PredictionInstance> PredictCognitiveLoadAsync(
            Guid userId,
            DateTime targetDate);
        
        /// <summary>
        /// Records actual outcome for a prediction
        /// </summary>
        Task RecordActualOutcomeAsync(
            Guid predictionId,
            double actualValue);
    }
}
